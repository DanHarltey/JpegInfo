namespace JpegInfo
{
    public class JpegHeaders
    {
        /// <summary>
        /// Header that contains ???
        /// </summary>
        public JfifHeader JfifHeader { get; internal set; }

        /// <summary>
        /// jpeg image dimensions in pixels
        /// </summary>
        public Dimensions Dimensions { get; internal set; }
    }
}
