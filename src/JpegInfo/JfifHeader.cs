namespace JpegInfo
{
    using System;

    public class JfifHeader
    {
        private const int MinHeaderSize = 14;

        protected JfifHeader(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (JfifHeader.MinHeaderSize > buffer.Length)
            {
                throw new ArgumentException($"Must contain at at least {JfifHeader.MinHeaderSize} bytes.", nameof(buffer));
            }

            // first 5 bytes are 'JFIF'#0 (0x4a, 0x46, 0x49, 0x46, 0x00)

            this.MajorRevision = buffer[5];
            this.MinorRevision = buffer[6];
            this.Density = (Density)buffer[7];
            this.XDensity =  Jpeg.ReadLength(buffer, 8);
            this.YDensity = Jpeg.ReadLength(buffer, 10);
            this.ThumbnailWidth = buffer[12];
            this.ThumbnailHeight = buffer[13];
        }

        public static JfifHeader Create(byte[] buffer)
        {
            JfifHeader jfif = null;

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (JfifHeader.MinHeaderSize > buffer.Length)
            {
                jfif = null;
            }
            else
            {
                jfif = new JfifHeader(buffer);
            }

            return jfif;
        }

        public byte MajorRevision { get; }
        public byte MinorRevision { get; }
        public Density Density { get; }
        public ushort XDensity { get; }
        public ushort YDensity { get; }
        public byte ThumbnailWidth { get; }
        public byte ThumbnailHeight { get; }
    }
}
