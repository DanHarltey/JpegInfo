using System.Collections.Generic;

namespace JpegInfo
{
    public class JpegHeaders
    {
        private readonly List<string> comments = new List<string>();

        /// <summary>
        /// Header that contains ???
        /// </summary>
        public JfifHeader JfifHeader { get; internal set; }

        public ExifHeaders ExifHeaders { get; internal set; }

        /// <summary>
        /// jpeg image dimensions in pixels
        /// </summary>
        public Dimensions Dimensions { get; internal set; }

        public IReadOnlyList<string> Comments => this.comments;

        internal void AddComment(string comment) => this.comments.Add(comment);
    }
}
