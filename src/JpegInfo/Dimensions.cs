namespace JpegInfo
{
    using System;

    public struct Dimensions
    {
        private const int MinHeaderSize = 9;

        internal Dimensions(byte[] buffer)
        {
            if(buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if(Dimensions.MinHeaderSize > buffer.Length)
            {
                throw new ArgumentException($"Must contain at at least {Dimensions.MinHeaderSize} bytes.", nameof(buffer));
            }

            this.Height = JpegInfo.ReadLength(buffer, 1);
            this.Width = JpegInfo.ReadLength(buffer, 3);
        }

        /// <summary>
        /// The width of the image in pixels. Always above 0.
        /// </summaWidthry>
        public ushort Width { get; }

        /// <summary>
        /// The height of the image in pixels. Always above 0.
        /// </summary>
        public ushort Height { get; }
    }
}
