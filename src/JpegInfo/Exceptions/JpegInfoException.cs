namespace JpegInfo.Exceptions
{
    using System;

    public abstract class JpegInfoException : ArgumentException
    {
        protected JpegInfoException(string message)
            : base(message, "jpegStream")
        {
        }

        protected JpegInfoException(string message, Exception innerException)
            : base(message, "jpegStream", innerException)
        {
        }
    }
}
