namespace JpegInfo
{
    using System;

    /// <summary>
    /// This exception is thrown when the passed stream does not conform to the jpeg standard.
    /// </summary>
    public class NotValidJpegException : ArgumentException
    {
        internal NotValidJpegException()
            :base()
        {
        }

        internal NotValidJpegException(string message)
            :base(message)
        {
        }

        internal NotValidJpegException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
