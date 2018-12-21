namespace JpegInfo
{
    using System;
    using System.Text;

    internal static class Comments
    {
        public static void Create(JpegHeaders jpegHeaders, byte[] buffer)
        {
            if (jpegHeaders == null)
            {
                throw new ArgumentNullException(nameof(jpegHeaders));
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            string comment = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            jpegHeaders.AddComment(comment);
        }
    }
}
