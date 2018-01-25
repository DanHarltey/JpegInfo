namespace JpegInfo
{
    using System;
    using System.IO;

    public static class JpegInfo
    {

        /// <summary>
        /// Reads the passed stream and returns the jpeg image dimensions in pixels.
        /// It stops reading the stream when the dimensions are found.
        /// </summary>
        /// <param name="jpegStream"></param>
        /// <exception cref="NotValidJpegException">This exception is thrown when the passed stream does not conform to the jpeg standard.</exception>
        /// <returns>the image dimensions in pixels</returns>
        public static Dimensions GetDimensions(Stream jpegStream)
        {
            JpegInfo.ThrowIfNotJpeg(jpegStream);

            Dimensions imageDetails = new Dimensions();

            byte[] headerBuffer = new byte[4];
            ushort length;

            do
            {
                JpegInfo.CheckedRead(jpegStream, headerBuffer);
                length = JpegInfo.ReadLength(headerBuffer, 2);

                if (headerBuffer[0] != 0xff)
                {
                    // because all headers have to start with 0xff
                    throw new NotValidJpegException();
                }

                //TODO make this a seek if we do not need to read the data
                byte[] headerData = new byte[length - 2];
                JpegInfo.CheckedRead(jpegStream, headerData);

                if (headerBuffer[1] == Markers.SOF0 || headerBuffer[1] == Markers.SOF2)
                {
                    imageDetails = new Dimensions(headerData);
                }
            }
            while (headerBuffer[1] != Markers.SOS && imageDetails.Height == 0);

            if(imageDetails.Height == 0)
            {
                // because it has no Start Of Frame header
                throw new NotValidJpegException();
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
            JpegInfo.CheckedRead(jpegStream, buffer);

            if (buffer[0] != 0xff || buffer[1] != 0xd8)
            {
                // jpegs have to start with 0xff, 0xd8 because them the rules!
                throw new NotValidJpegException();
            }
        }

        internal static ushort ReadLength(byte[] buffer, int index)
        {
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
            int read = jpegStream.Read(buffer, 0, buffer.Length);

            if (buffer.Length != read)
            {
                throw new NotValidJpegException();
            }
        }
    }
}
