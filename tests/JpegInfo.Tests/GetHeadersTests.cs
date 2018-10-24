namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class GetHeadersTests
    {
        [Fact]
        public void Throws_AgrNull_When_Null_Stream()
        {
            Assert.Throws<ArgumentNullException>(() => Jpeg.GetHeaders(null));
        }

        [Fact]
        public void Throws_StreamTooSmall_When_Stream_Empty()
        {
            // empty memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.Throws<Exceptions.StreamTooSmallException>(() => Jpeg.GetHeaders(stream));
            }
        }

        [Fact]
        public void Throws_InvalidJpeg_When_FileMarker_Missing()
        {
            byte[] missingHeader = new byte[10];

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all bytes are 0 to start with
                Assert.Throws<Exceptions.InvalidJpegException>(() => Jpeg.GetHeaders(stream));
            }


            // set the first byte to be valid to make sure it is checking second byte as well
            missingHeader[0] = 0xff;

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all 0 bytes to start with
                Assert.Throws<Exceptions.InvalidJpegException>(() => Jpeg.GetHeaders(stream));
            }
        }

        [Fact]
        public void Throws_StreamTooSmallException_When_Stream_Too_Small()
        {
            /*
             * This test make sure that it is performing checked reads.
             * It should throw when it tries to read but the stream is empty
             */

            // this has the file marker but no oihers so it should throw when trying to read the next header
            byte[] shortImage = new byte[]
            {
                0xff,
                Markers.FileMarker
            };

            using (MemoryStream stream = new MemoryStream(shortImage))
            {
                Assert.Throws<Exceptions.StreamTooSmallException>(() => Jpeg.GetHeaders(stream));
            }
        }
    }
}