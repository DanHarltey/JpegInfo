namespace JpegInfo.Exceptions
{
    using System;

    /// <summary>
    /// This exception is thrown when the passed stream does not conform to the jpeg standard.
    /// </summary>
    public class InvalidJpegException : JpegInfoException
    {
        internal InvalidJpegException(string message)
            : base(message)
        {
        }

        internal InvalidJpegException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}