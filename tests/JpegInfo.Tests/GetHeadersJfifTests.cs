namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class GetHeadersJfifTests
    {
        [Fact]
        public void Should_Read_No_Density()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF0_1280x853.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                JfifHeader jfif = headers.JfifHeader;

                Assert.Equal(1, jfif.MajorRevision);
                Assert.Equal(2, jfif.MinorRevision);

                Assert.Equal( Density.None, jfif.Density);
                Assert.Equal(1, jfif.XDensity);
                Assert.Equal(1, jfif.YDensity);

                Assert.Equal(0, jfif.ThumbnailHeight);
                Assert.Equal(0, jfif.ThumbnailWidth);
            }
        }

        [Fact]
        public void Should_Read_Dots_PerInch()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF2_800x600.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                JfifHeader jfif = headers.JfifHeader;

                Assert.Equal(1, jfif.MajorRevision);
                Assert.Equal(1, jfif.MinorRevision);

                Assert.Equal(Density.DotPerInch, jfif.Density);
                Assert.Equal(300, jfif.XDensity);
                Assert.Equal(300, jfif.YDensity);

                Assert.Equal(0, jfif.ThumbnailHeight);
                Assert.Equal(0, jfif.ThumbnailWidth);
            }
        }

        [Fact]
        public void Should_Allow_No_JFIF()
        {
            // this image has I higher number of components than normal because of photometric interpretation CMYK
            using (Stream fileStream = File.OpenRead(@"testimages\CMYK_SOF0_5100x3300.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);
                JfifHeader jfif = headers.JfifHeader;

                Assert.Null(jfif);
            }
        }

        [Fact]
        public void Should_Read_From_Image_With_Bad_Segment_Size()
        {
            // this image has a bad segment size that must be recovered from, it also has LOADS of metadata
            using (Stream fileStream = File.OpenRead(@"testimages\Bad_Segment_Size_4596x3418.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                JfifHeader jfif = headers.JfifHeader;

                Assert.Equal(1, jfif.MajorRevision);
                Assert.Equal(2, jfif.MinorRevision);

                Assert.Equal(Density.DotPerInch, jfif.Density);
                Assert.Equal(72, jfif.XDensity);
                Assert.Equal(72, jfif.YDensity);

                Assert.Equal(0, jfif.ThumbnailHeight);
                Assert.Equal(0, jfif.ThumbnailWidth);
            }
        }
    }
}