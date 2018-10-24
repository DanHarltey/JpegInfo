namespace JpegInfo.Tests
{
    using System.IO;
    using System.Linq;
    using Xunit;

    public class FindNextSegmentTests
    {
        [Fact]
        public void Should_Read_Stream_Until_Marker_Found()
        {
            const int TestSize = 10;

            for (int i = 0; i < TestSize; i++)
            {
                // this represents the stream of an image
                byte[] imageBuffer = new byte[TestSize + 3];

                // number the bytes so we can test they where copied of the stream
                for (byte j = 0; j < imageBuffer.Length; j++)
                {
                    imageBuffer[j] = j;
                }

                // the method should find this marker
                imageBuffer[i] = Jpeg.SectionStartMarker;

                byte[] headerBuffer = imageBuffer.Take(4).ToArray();

                using (MemoryStream jpegStream = new MemoryStream(imageBuffer, headerBuffer.Length, imageBuffer.Length - headerBuffer.Length))
                {
                    Jpeg.FindNextSegment(jpegStream, headerBuffer);

                    // method should have made the marker the first byte in the array
                    Assert.Equal(Jpeg.SectionStartMarker, headerBuffer[0]);

                    // it should of filled the headerBuffer with bytes from the stream
                    Assert.Equal(i, jpegStream.Position);

                    for (int j = 1; j < headerBuffer.Length; j++)
                    {
                        // make sure the bytes should the stream are in the headerBuffer
                        Assert.Equal(i + j, headerBuffer[j]);
                    }
                }
            }
        }

        [Fact]
        public void Throws_BadSegmentSize_When_Stream_Ends()
        {
            byte[] headerBuffer = new byte[4];

            // the stream is empty so it should throw when trying to get next bytes
            using (MemoryStream jpegStream = new MemoryStream(new byte[0]))
            {
                Assert.Throws<Exceptions.BadSegmentSizeException>(() => Jpeg.FindNextSegment(jpegStream, headerBuffer));
            }
        }
    }
}
