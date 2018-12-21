namespace JpegInfo
{
    using System;
    using System.Linq;

    using System.Collections.Generic;
    using System.Text;

    public class ExifHeaders
    {
        private const int MinHeaderSize = 4;

        public static ExifHeaders Create(byte[] buffer)
        {
            ExifHeaders exif = null;

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (ExifHeaders.MinHeaderSize > buffer.Length
                || buffer[0] != 0x45 // 'E'
                || buffer[1] != 0x78 // 'x'
                || buffer[2] != 0x69 // 'i'
                || buffer[3] != 0x66 // 'f'
                || buffer[4] != 0x00
                || buffer[5] != 0x00)
            {
                exif = null;
            }
            else
            {
                // data aglign
                if(buffer[5] == 0x49 && buffer[6] == 0x49)
                {
                    // dont reverse
                }
                else if(buffer[5] == 0x4d && buffer[6]== 0x4d)
                {
                    // do revser
                }
                else
                {
                    // error
                }

                    var asa = buffer.Select(x => (char)x).ToList();

                exif = null;
            }

            return exif;
        }
    }
}
