namespace JpegInfo.Exceptions
{
    using System;

    /// <summary>
    /// This exception is thrown when an attempt to recover from a bad segment size fails due to not finding a section marker before the end of the stream.
    /// </summary>
    public class BadSegmentSizeException : InvalidJpegException
    {
        internal BadSegmentSizeException(Exception innerException)
            :base("The jpeg stream contained an erroneous segment size that could not be recovered from.", innerException)
        {
        }
    }
}