namespace KavisProxy.Core
{
    public class ByteBuffer
    {
        private MemoryStream Data;
        private int Index = 0;

        public ByteBuffer() { }
        public ByteBuffer(byte[] data)
        {
            Data = new MemoryStream();//data.ToList();
        }

        public void WriteByte(byte data)
        {
            Data.WriteByte(data);
        }

        public void WriteBytes(byte[] data)
        {
            Data.Write(data,(int)Data.Length, data.Length);
        }

        public void WriteBool(bool value)
        {
            if (value)
            {
                WriteByte(0x01);
            }
            else
            {
                WriteByte(0x00);
            }
        }

        public void WriteByteInt(sbyte value)
        {
            WriteByte((byte)value);
        }

        public void WriteByteUnsigned(byte value) //WriteByte and WriteByteUnsigned is equal
        {
            WriteByte(value);
        }

        public void WriteShort(short value)
        {
            
        }

        public void WriteUnsignedShort(ushort value)
        {

        }

        public void WriteInt(int value)
        {

        }

        public void WriteLong(long value)
        {

        }

        public void WriteFloat(float value)
        {

        }

        public void WriteDouble(double value)
        {

        }

        public void WriteUUID(Guid value)
        {
            WriteBytes(value.ToByteArray());
        }

        public void WritePosition(int x,int y, int z)
        {
            var X = ((x & 0x3FFFFFF) << 38);
            var Y = ((y & 0xFFF) << 26);
            var Z = (z & 0x3FFFFFF);
            
        }

        public byte ReadByte(bool AddToIndex = true)
        {
            if (Data.Length < 1 + Index)
            {
                throw new EndOfStreamException("End of stream reached.");
            }

            Data.Position = Index;
            var value = (byte)Data.ReadByte();
            if (AddToIndex)
            {
                Index += 1;
            }
            return value;
        }

        public byte[] ReadBytes(int count, bool AddToIndex = true)
        {
            if (Data.Length < count + Index)
            {
                throw new EndOfStreamException("End of stream reached.");
            }

            var array = new byte[count];
            Data.Position = Index;
            Data.Read(array, 0, count);
            if (AddToIndex)
            {
                Index += count;
            }
            return array;
        }

        public bool ReadBool()
        {
            byte data = ReadByte();
            return data == 0x01;
        }

        public sbyte ReadByteInt()
        {
            /*
            sbyte a = -129; //Error
            sbyte a = -128;
            sbyte a = 127;
            sbyte a = 128; //Error
            */

            return (sbyte)ReadByte();
        }

        public byte ReadByteUnsigned()
        {
            /*
            byte a = -1; //Error
            byte a = 0;
            byte a = 255;
            byte a = 256; //Error
            */
            return ReadByte();
        }

        public short ReadShort()
        {
            /*
            short a = -32769; //Error
            short a = -32768;
            short a = 32767;
            short a = 32768; //Error
            */

            return BitConverter.ToInt16(ReadBytes(2).AsSpan());
        }

        public ushort ReadUnsignedShort()
        {
            /*
            ushort a = -1; //Error
            ushort a = 0;
            ushort a = 65535;
            ushort a = 65536; //Error
            */
            return BitConverter.ToUInt16(ReadBytes(2).AsSpan());
        }

        public int ReadInt()
        {
            /*
            int a = -2147483649; //Error
            int a = -2147483648;
            int a = 2147483647;
            int a = 2147483648; //Error
            */
            return BitConverter.ToInt32(ReadBytes(4).AsSpan());
        }

        public long ReadLong()
        {
            /*
            long a = -9223372036854775809; //Error
            long a = -9223372036854775808;
            long a = 9223372036854775807;
            long a = 9223372036854775808; //Error
            */
            return BitConverter.ToInt64(ReadBytes(8).AsSpan());
        }

        public float ReadFloat()
        {
            //IEEE 754, 4*8 => 32
            return BitConverter.ToSingle(ReadBytes(4).AsSpan());
        }

        public double ReadDouble()
        {
            //IEE 754, 8*8 => 64
            return BitConverter.ToDouble(ReadBytes(8).AsSpan());
        }

        public Guid ReadUUID()
        {
            return new Guid(ReadBytes(16).AsSpan());
        }


    }
}
