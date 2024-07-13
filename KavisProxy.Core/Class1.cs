namespace KavisProxy.Core
{
    public class ByteBuffer
    {
        private List<byte> Data = new(); //it's need to be changed
        private int Index = 0;

        public ByteBuffer() { }
        public ByteBuffer(byte[] data)
        {
            Data = data.ToList();
        }

        public void WriteBytes(byte[] data)
        {
            Data.AddRange(data);
        }

        public byte[] ReadBytes(int count, bool AddToIndex = true)
        {
            if (Data.Count < count + Index)
            {
                throw new StackOverflowException();
            }
            var array = Data.GetRange(Index, count);
            if (AddToIndex)
            {
                Index += count;
            }
            return array.ToArray();
        }

        public bool ReadBool()
        {
            byte data = ReadBytes(1)[0];
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

            return (sbyte)ReadBytes(0)[0];
        }

        public byte ReadByteUnsigned()
        {
            /*
            byte a = -1; //Error
            byte a = 0;
            byte a = 255;
            byte a = 256; //Error
            */
            return ReadBytes(1)[0];
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
