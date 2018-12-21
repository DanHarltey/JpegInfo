namespace JpegInfo.Tests
{
    using System;
    using System.IO;
    using Xunit;

    public class GetHeadersCommentsTests
    {
        [Fact]
        public void Should_Return_Empty_Comments()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF0_1280x853.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.Empty(headers.Comments);
            }
        }

        [Fact]
        public void Should_Read_Comment()
        {
            using (Stream fileStream = File.OpenRead(@"testimages\SOF2_800x600.jpg"))
            {
                JpegHeaders headers = Jpeg.GetHeaders(fileStream);

                Assert.NotEmpty(headers.Comments);
                Assert.Equal("MOGRIFICAT", headers.Comments[0]);
            }
        }
    }
}