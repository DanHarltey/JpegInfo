namespace JpegInfo.Exceptions
{
    /// <summary>
    /// This expectation is thrown when the passed stream is too small and finishes before the end of the Jpeg headers. 
    /// </summary>
    public class StreamTooSmallException : JpegInfoException
    {
        internal StreamTooSmallException()
            :base("The passed stream is too small and finishes before the end of the Jpeg headers.")
        {
        }
    }
}
