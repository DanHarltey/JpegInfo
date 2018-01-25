namespace JpegInfo
{
    internal static class Markers 
    {
        /// <summary>
        /// Present at start of Jpeg file
        /// </summary>
        internal const byte FileMarker = 0xd8;

        /// <summary>
        /// Start Of Frame(baseline JPEG)
        /// </summary>
        internal const byte SOF0 = 0xc0;
        /// <summary>
        /// Start Of Frame 2 ?
        /// </summary>
        internal const byte SOF2 = 0xc2;

        /// <summary>
        /// SOS(Start Of Scan) always the last marker before image data starts
        /// </summary>
        internal const byte SOS = 0xda;
    }
}

////           internal static readonly byte APP0_Marker = 0xe0;

////    // DQT(Define Quantization Table) marker:
////    internal static readonly byte DQT_Marker = 0xdb;

////    // Start Of Frame(baseline JPEG)
////    internal static readonly byte SOF0_Marker = 0xc0;

////    //  SOF2 usually unsupported
////    internal static readonly byte SOF2_Marker = 0xc2;

////    // Define Huffman Table
////    internal static readonly byte DHT_Marker = 0xc4;

////    // SOS(Start Of Scan)
////    internal static readonly byte SOS_Marker = 0xda;


////    // Other APPx segments
////    internal static readonly byte APPx1_Marker = 0xe1;
////    internal static readonly byte APPx2_Marker = 0xe2;
////    internal static readonly byte APPx5_Marker = 0xec;
////    internal static readonly byte APPx3_Marker = 0xed;
////    internal static readonly byte APPx4_Marker = 0xee;

////    // COM Comment, may be ignored
////    internal static readonly byte COM_Marker = 0xfe;


////    // DRI(Define Restart Interval) 
////    internal static readonly byte DRI_Marker = 0xdd;
////}
////}
