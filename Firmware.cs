using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWManager
{
    public class Firmware
    {
        private const int FIRMWARE_DESCRIPTION_SIZE = 40;
        private const int TILE_SIZE = 86 * 48 * 2;
        private const int HEADER_SIZE = 24;
        private const int PARTITION_HEADER_SIZE = 32;

        public string FirmwareDescription { get; set; } // Max 40 Char.

        // [EditorAttribute(typeof(SaveFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Header { get; set; } // "ODROIDGO_FIRMWARE_V00_01";

        [Browsable(false)]
        public byte[] TileBytes { get; set; }


        Image tile = null;

        [EditorAttribute(typeof(CustomImageEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public Image Tile
        {
            get
            {
                if (tile == null && TileBytes != null)
                {
                    tile = ImageConversation.ByteToImage(86, 48, TileBytes);
                }

                return tile;
            }
            set
            {
                if (value != null)
                {
                    if ((value.Width != 86) || (value.Height != 48))
                    {
                        value = ImageConversation.ReseizeImage(86,48,value);
                        System.Windows.Forms.MessageBox.Show("The Image was resized to 86x48 Pixel.");
                    }


                    TileBytes = ImageConversation.ImageToByte(value);
                }

                tile = value;
                
            }
        }

        public List<Partition> Partitions { get; set; }

        public byte[] GetBytes()
        {
            // Calculate total size
            int size = HEADER_SIZE + FIRMWARE_DESCRIPTION_SIZE + TILE_SIZE + 4;
            foreach (var partition in this.Partitions)
            {
                size += PARTITION_HEADER_SIZE + partition.Bytes.Length;
            }

            Byte[] buffer = new byte[size];

            int offset = 0;

            Encoding.Default.GetBytes(this.Header, 0, this.Header.Length, buffer, offset);
            offset += HEADER_SIZE;


            Encoding.Default.GetBytes(this.FirmwareDescription, 0, this.FirmwareDescription.Length, buffer, offset);
            offset += FIRMWARE_DESCRIPTION_SIZE;

            System.Array.Copy(this.TileBytes, 0, buffer, offset, TILE_SIZE);
            offset += TILE_SIZE;

            foreach (var partition in this.Partitions)
            {
                partition.GetBytes(buffer, offset);
                offset += PARTITION_HEADER_SIZE + partition.Bytes.Length;
            }

            Crc32.CalculateAndAppend(buffer);

            return buffer;
        }


        public static Firmware Parse(byte[] buffer)
        {
            Firmware firmware = new Firmware();
            firmware.Partitions = new List<Partition>();

            int offset = 0;

            firmware.Header = Encoding.Default.GetString(buffer, offset, HEADER_SIZE);
            offset += HEADER_SIZE;

            firmware.FirmwareDescription = Encoding.Default.GetString(buffer, offset, FIRMWARE_DESCRIPTION_SIZE);
            offset += FIRMWARE_DESCRIPTION_SIZE;

            firmware.TileBytes = new byte[TILE_SIZE];
            System.Array.Copy(buffer, offset, firmware.TileBytes, 0, TILE_SIZE);
            offset += TILE_SIZE;

            while (offset + 4 < buffer.Length)
            {
                Partition partition = Partition.Parse(buffer, offset);
                offset += PARTITION_HEADER_SIZE + partition.Bytes.Length;

                firmware.Partitions.Add(partition);
            }

            UInt32 crc32 = BitConverter.ToUInt32(buffer, offset);

            return firmware;
        }


        public static Firmware ReadFromStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return Firmware.Parse(ms.ToArray());
            }
        }

        public void WriteToStream(Stream stream)
        {
            byte[] bytes = this.GetBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

    }
}
