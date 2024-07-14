using System;
using System.Numerics;
using System.Text;

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
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteUnsignedShort(ushort value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteInt(int value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteLong(long value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteFloat(float value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteDouble(double value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        public void WriteUUID(Guid value)
        {
            WriteBytes(value.ToByteArray());
        }

        public void WritePosition(Vector3Int position)
        {
            long value = ((long)(position.X & 0x3FFFFFF) << 38) | ((long)(position.Y & 0xFFF) << 26) | (position.Z & 0x3FFFFFF);
            WriteLong(value);
        }

        public void WriteVarInt(int value)
        {
            while ((value & -128) != 0)
            {
                WriteByte((byte)(value & 127 | 128));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        public void WriteVarLong(long value)
        {
            while ((value & -128L) != 0L)
            {
                WriteByte((byte)(value & 127L | 128L));
                value >>= 7;
            }
            WriteByte((byte)value);
        }

        public void WriteAngle(byte angle)
        {
            WriteByte(angle);
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteVarInt(bytes.Length);
            WriteBytes(bytes);
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

        public int ReadVarInt()
        {
            int numRead = 0;
            int result = 0;
            byte read;
            do
            {
                read = ReadByte();
                int value = read & 0b01111111;
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 5)
                {
                    throw new Exception("VarInt is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public long ReadVarLong()
        {
            int numRead = 0;
            long result = 0;
            byte read;
            do
            {
                read = ReadByte();
                long value = read & 0b01111111L;
                result |= value << (7 * numRead);

                numRead++;
                if (numRead > 10)
                {
                    throw new Exception("VarLong is too big");
                }
            } while ((read & 0b10000000) != 0);

            return result;
        }

        public Vector3Int ReadPosition()
        {
            long value = ReadLong();
            int x = (int)(value >> 38);
            int y = (int)((value >> 26) & 0xFFF);
            int z = (int)(value << 38 >> 38); // sign-extend
            return new Vector3Int(x, y, z);
        }

        public byte ReadAngle()
        {
            return ReadByte();
        }

        public string ReadString()
        {
            int lenght = ReadVarInt();
            var str = ReadBytes(lenght);
            return Encoding.UTF8.GetString(str);
        }
    }
}
