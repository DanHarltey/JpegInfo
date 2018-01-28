namespace JpegInfo
{
    using global::JpegInfo.Exceptions;
    using System;
    using System.IO;

    public static class Jpeg
    {
        internal const byte SectionStartMarker = 0xff;

        /// <summary>
        /// Reads the passed stream and returns the jpeg image dimensions in pixels.
        /// It stops reading the stream when the dimensions are found.
        /// </summary>
        /// <param name="jpegStream"></param>
        /// <exception cref="InvalidJpegException">This exception is thrown when the passed stream does not conform to the jpeg standard.</exception>
        /// <exception cref="StreamTooSmallException">This exception is thrown when the passed stream does not contain the required jpeg headers before the end of the stream.</exception>
        /// <exception cref="BadSegmentSizeException">This exception is thrown when an attempt to recover from a bad segment size fails due to not finding a section marker before the end of the stream.</exception>
        /// <returns>the image dimensions in pixels</returns>
        public static Dimensions GetDimensions(Stream jpegStream)
        {
            Jpeg.ThrowIfNotJpeg(jpegStream);

            Dimensions imageDetails = new Dimensions();

            byte[] headerBuffer = new byte[4];

            do
            {
                Jpeg.CheckedRead(jpegStream, headerBuffer);

                if (headerBuffer[0] != Jpeg.SectionStartMarker)
                {
                    Jpeg.FindNextSegment(jpegStream, headerBuffer);
                }

                ushort length = Jpeg.ReadLength(headerBuffer, 2);

                //TODO make this a seek if we do not need to read the data
                byte[] headerData = new byte[length - 2];
                Jpeg.CheckedRead(jpegStream, headerData);

                if (headerBuffer[1] == Markers.SOF0 || headerBuffer[1] == Markers.SOF2)
                {
                    imageDetails = new Dimensions(headerData);
                }
            }
            while (headerBuffer[1] != Markers.SOS && imageDetails.Height == 0);

            if (imageDetails.Height == 0)
            {
                // because it has no Start Of Frame header
                throw new InvalidJpegException("Failed to find Start Of Frame header.");
            }

            return imageDetails;
        }

        private static void ThrowIfNotJpeg(Stream jpegStream)
        {
            if (jpegStream == null)
            {
                throw new ArgumentNullException(nameof(jpegStream));
            }

            byte[] buffer = new byte[2];
            Jpeg.CheckedRead(jpegStream, buffer);

            if (buffer[0] != Jpeg.SectionStartMarker || buffer[1] != Markers.FileMarker)
            {
                // jpegs have to start with 0xff, 0xd8 because them the rules!
                throw new InvalidJpegException("Invalided JPEG/JFIF. Does not start with SOI marker.");
            }
        }

        internal static ushort ReadLength(byte[] buffer, int index)
        {
            //TODO maybe try this to see if quicker (short)((bytes[8] << 8) | bytes[9]);
            byte[] lengthBuffer = new byte[2];
            Array.Copy(buffer, index, lengthBuffer, 0, 2);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthBuffer);
            }

            ushort length = BitConverter.ToUInt16(lengthBuffer, 0);

            return length;
        }

        private static void CheckedRead(Stream jpegStream, byte[] buffer)
        {
            Jpeg.CheckedRead(jpegStream, buffer, 0);
        }

        private static void CheckedRead(Stream jpegStream, byte[] buffer, int startIndex)
        {
            int toRead = buffer.Length - startIndex;

            int read = jpegStream.Read(buffer, startIndex, toRead);

            if (toRead != read)
            {
                throw new StreamTooSmallException();
            }
        }

        internal static void FindNextSegment(Stream jpegStream, byte[] headerBuffer)
        {
            /*
             * Well I'll be.
             * Really this is no longer a valid jpeg at all. 
             * The last segment size was incorrect as we should have found a 0xff for the next segment but we did not. 
             * We could have jumped anywhere in the file. Could still be in the same segment or jumped into the middle of another segment. 
             * We really have no idea where we are!
             * If it was up to me we would just throw invalid jpeg here.
             *
             * However, libjpeg and most other jpeg parsing libraries do not throw. 
             * So, in order to keep compatibly with them we do what they do. We go byte by byte looking for 0xff.
             *
             * This results in a silent failure. We ignore and throw anyway the data we are reading and/or missed.
             */

            try
            {
                /* 
                 * We read 4 bytes at a time skiping over any non-FF bytes.
                 * This code is only used we the jpeg stream is invalid so not performance critical.
                 */

                int i = 0;

                while (headerBuffer[i] != Jpeg.SectionStartMarker)
                {
                    ++i;

                    if (i == headerBuffer.Length)
                    {
                        Jpeg.CheckedRead(jpegStream, headerBuffer, 0);
                        i = 0;
                    }
                }

                if (i != 0)
                {
                    // found the marker in the data we have move the data down the buffer
                    Buffer.BlockCopy(headerBuffer, i, headerBuffer, 0, headerBuffer.Length - i);

                    // fill the rest of the buffer with data from the stream
                    Jpeg.CheckedRead(jpegStream, headerBuffer, headerBuffer.Length - i);
                }
            }
            catch (StreamTooSmallException ex)
            {
                // we shit we ran into the end of the stream, we could not recover from the bad jump
                throw new BadSegmentSizeException(ex);
            }
        }
    }
}
