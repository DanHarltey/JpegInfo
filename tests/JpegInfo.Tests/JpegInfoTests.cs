namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class JpegInfoTests
    {

        [Fact]
        public void GetDimensions_Throws_AgrNull_When_Null_Stream()
        {
            Assert.Throws<ArgumentNullException>(() => JpegInfo.GetDimensions(null));
        }

        [Fact]
        public void GetDimensions_Throws_NotValidJpeg_When_Stream_Empty()
        {
            // empty memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }
        }

        [Fact]
        public void GetDimensions_Throws_NotValidJpeg_When_FileMarker_Missing()
        {
            byte[] missingHeader = new byte[10];

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all bytes are 0 to start with
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }


            // set the first byte to be valid to make sure it is checking second byte as well
            missingHeader[0] = 0xff;

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all 0 bytes to start with
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }
        }

        [Fact]
        public void GetDimensions_Throws_NotValidJpeg_When_Header()
        {
            byte[] missingHeader = new byte[10];

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all bytes are 0 to start with
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }

            // set the first byte to be valid to make sure it is checking second byte as well
            missingHeader[0] = 0xff;

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all 0 bytes to start with
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }
        }

        [Fact]
        public void GetDimensions_Throws_NotValidJpeg_When_Stream_To_Short()
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
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }
        }

        [Fact]
        public void GetDimensions_Throws_NotValidJpeg_When_No_HeaderMarker()
        {
            byte[] badImage = new byte[]
            {
                0xff,
                Markers.FileMarker,
                0xfd, // this should be 0xff to mark an header
                // three extra bytes to makr sure the read does not fail
                0x00,
                0x00,
                0x00
            };

            using (MemoryStream stream = new MemoryStream(badImage))
            {
                Assert.Throws<NotValidJpegException>(() => JpegInfo.GetDimensions(stream));
            }
        }

        [Fact]
        public void GetDimensions_Get_Dimensions_From_SOF0()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF0_1280x853.jpg"))
            {
                Dimensions dimensions = JpegInfo.GetDimensions(fileStream);

                Assert.Equal(1280, dimensions.Width);
                Assert.Equal(853, dimensions.Height);
            }
        }

        [Fact]
        public void GetDimensions_Get_Dimensions_From_SOF2()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF2_800x600.jpg"))
            {
                Dimensions dimensions = JpegInfo.GetDimensions(fileStream);

                Assert.Equal(800, dimensions.Width);
                Assert.Equal(600, dimensions.Height);
            }
        }
    }
}