namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class GetDimensionsTests
    {
        [Fact]
        public void Throws_AgrNull_When_Null_Stream()
        {
            Assert.Throws<ArgumentNullException>(() => Jpeg.GetDimensions(null));
        }

        [Fact]
        public void Throws_StreamTooSmall_When_Stream_Empty()
        {
            // empty memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                Assert.Throws<Exceptions.StreamTooSmallException>(() => Jpeg.GetDimensions(stream));
            }
        }

        [Fact]
        public void Throws_InvalidJpeg_When_FileMarker_Missing()
        {
            byte[] missingHeader = new byte[10];

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all bytes are 0 to start with
                Assert.Throws<Exceptions.InvalidJpegException>(() => Jpeg.GetDimensions(stream));
            }


            // set the first byte to be valid to make sure it is checking second byte as well
            missingHeader[0] = 0xff;

            using (MemoryStream stream = new MemoryStream(missingHeader))
            {
                // all 0 bytes to start with
                Assert.Throws<Exceptions.InvalidJpegException>(() => Jpeg.GetDimensions(stream));
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
                Assert.Throws<Exceptions.StreamTooSmallException>(() => Jpeg.GetDimensions(stream));
            }
        }

        [Fact]
        public void Should_Read_From_SOF0()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF0_1280x853.jpg"))
            {
                Dimensions dimensions = Jpeg.GetDimensions(fileStream);

                Assert.Equal(1280, dimensions.Width);
                Assert.Equal(853, dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_SOF2()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF2_800x600.jpg"))
            {
                Dimensions dimensions = Jpeg.GetDimensions(fileStream);

                Assert.Equal(800, dimensions.Width);
                Assert.Equal(600, dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_SOF0_With_CMYK()
        {
            // this image has I higher number of components than normal because of photometric interpretation CMYK
            using (Stream fileStream = File.OpenRead(@"testimages\CMYK_SOF0_5100x3300.jpg"))
            {
                Dimensions dimensions = Jpeg.GetDimensions(fileStream);

                Assert.Equal(5100, dimensions.Width);
                Assert.Equal(3300, dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_Image_With_Bad_Segment_Size()
        {
            // this image has a bad segment size that must be recovered from, it also has LOADS of metadata
            using (Stream fileStream = File.OpenRead(@"testimages\Bad_Segment_Size_4596x3418.jpg"))
            {
                Dimensions dimensions = Jpeg.GetDimensions(fileStream);

                Assert.Equal(4596, dimensions.Width);
                Assert.Equal(3418, dimensions.Height);
            }
        }
    }
}