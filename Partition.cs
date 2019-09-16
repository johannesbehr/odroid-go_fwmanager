using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWManager
{
    public class Partition
    {
        public byte Type { get; set; }
        public byte Subtype { get; set; }
        public byte Reserved0 { get; set; }
        public byte Reserved1 { get; set; }
        public string Label { get; set; }
        public uint Flags { get; set; }
        public uint Length { get; set; }

        [EditorAttribute(typeof(CustomBinaryEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public byte[] Bytes { get; set; }

        public override string ToString()
        {
            return "Partition: " + Label;
        }

        Byte[] GetBytes(){
            throw new NotImplementedException();
        }

        public static Partition Parse(byte[] data, int offset) {

            Partition partition = new Partition();
            partition.Type = data[offset];
            offset++;

            partition.Subtype = data[offset];
            offset++;

            partition.Reserved0 = data[offset];
            offset++;

            partition.Reserved1 = data[offset];
            offset++;

            partition.Label = Encoding.Default.GetString(data, offset, 16);
            offset+=16;

            partition.Flags = BitConverter.ToUInt32(data, offset);
            offset += 4;

            partition.Length = BitConverter.ToUInt32(data, offset);
            offset += 4;

            UInt32 dataLength =  BitConverter.ToUInt32(data, offset);
            offset += 4;

            partition.Bytes = new byte[dataLength];
            System.Array.Copy(data,offset, partition.Bytes, 0, dataLength);

            return partition;

        }

        public void GetBytes(byte[] buffer, int offset)
        {
            byte[] tmp;

            buffer[offset] = this.Type;
            offset++;

            buffer[offset] = this.Subtype;
            offset++;

            buffer[offset] = this.Reserved0;
            offset++;

            buffer[offset] = this.Reserved1;
            offset++;

            Encoding.Default.GetBytes(this.Label, 0, this.Label.Length, buffer, offset);
            offset += 16;

            tmp = BitConverter.GetBytes(this.Flags);
            System.Array.Copy(tmp, 0, buffer, offset, 4);
            offset += 4;

            tmp = BitConverter.GetBytes(this.Length);
            System.Array.Copy(tmp, 0, buffer, offset, 4);
            offset += 4;

            tmp = BitConverter.GetBytes(this.Bytes.Length);
            System.Array.Copy(tmp, 0, buffer, offset, 4);
            offset += 4;

            System.Array.Copy(this.Bytes, 0, buffer, offset, this.Bytes.Length);
            offset += this.Bytes.Length;
        }
    }
}
