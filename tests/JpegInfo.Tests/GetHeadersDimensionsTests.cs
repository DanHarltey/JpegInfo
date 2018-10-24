﻿namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class GetHeadersDimensionsTests
    {
        [Fact]
        public void Should_Read_From_SOF0()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF0_1280x853.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.Equal(1280, headers.Dimensions.Width);
                Assert.Equal(853, headers.Dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_SOF2()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF2_800x600.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.Equal(800, headers.Dimensions.Width);
                Assert.Equal(600, headers.Dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_SOF0_With_CMYK()
        {
            // this image has I higher number of components than normal because of photometric interpretation CMYK
            using (Stream fileStream = File.OpenRead(@"testimages\CMYK_SOF0_5100x3300.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.Equal(5100, headers.Dimensions.Width);
                Assert.Equal(3300, headers.Dimensions.Height);
            }
        }

        [Fact]
        public void Should_Read_From_Image_With_Bad_Segment_Size()
        {
            // this image has a bad segment size that must be recovered from, it also has LOADS of metadata
            using (Stream fileStream = File.OpenRead(@"testimages\Bad_Segment_Size_4596x3418.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.Equal(4596, headers.Dimensions.Width);
                Assert.Equal(3418, headers.Dimensions.Height);
            }
        }
    }
}