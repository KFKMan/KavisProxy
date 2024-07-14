using System.Text;

namespace KavisProxy.Core.Protocol;

public class ByteBuffer : IDisposable
{   
    Stream stream;
    int currrentPos = 0; // There can be a split into a write and read indices

    public ByteBuffer(Stream stream) => this.stream = stream;

    public ByteBuffer(byte[] data)
    {
        stream = new MemoryStream();
        WriteBytes(data);
        stream.Position = 0;
    }

    public void WriteByte(byte data) => stream.WriteByte(data);

    public void WriteBytes(byte[] data) => stream.Write(data, (int)stream.Length, data.Length);

    public void WriteBool(bool value) => WriteByte((byte)(value ? 1 : 0));

    public void WriteByteInt(sbyte value) => WriteByte((byte)value);

    public void WriteByteUnsigned(byte value) => WriteByte(value);

    public void WriteShort(short value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteUnsignedShort(ushort value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteInt(int value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteLong(long value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteFloat(float value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteDouble(double value) => WriteBytes(BitConverter.GetBytes(value));

    public void WriteUUID(Guid value) => WriteBytes(value.ToByteArray());

    public void WritePosition(Vec3i position)
    {
        var value = ((position.X & 0x3FFFFFFL) << 38) | ((position.Y & 0xFFFL) << 26) | (position.Z & 0x3FFFFFFL);
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

    public void WriteAngle(byte angle) => WriteByte(angle);

    public void WriteString(string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        WriteVarInt(bytes.Length);
        WriteBytes(bytes);
    }

    public byte ReadByte(bool AddToIndex = true)
    {
        if (stream.Length < 1 + currrentPos)
            throw new EndOfStreamException("End of stream reached.");

        stream.Position = currrentPos;
        var value = (byte)stream.ReadByte();
        if (AddToIndex)
            currrentPos += 1;

        return value;
    }

    public byte[] ReadBytes(int count, bool AddToIndex = true)
    {
        if (stream.Length < count + currrentPos)
            throw new EndOfStreamException("End of stream reached.");

        var array = new byte[count];
        stream.Position = currrentPos;
        stream.Read(array, 0, count);
        if (AddToIndex)
            currrentPos += count;

        return array;
    }

    public bool ReadBool()
    {
        byte data = ReadByte();
        return data == 0x01;
    }

    /// <returns>Value from -128 to 127 inclusive</returns>
    public sbyte ReadByteInt() => (sbyte)ReadByte();

    /// <returns>Value from 0 to 255 inclusive</returns>
    public byte ReadByteUnsigned() => ReadByte();

    /// <returns>Value from 32768 to 32767 inclusive</returns>
    public short ReadShort() => BitConverter.ToInt16(ReadBytes(2));

    /// <returns>Value from 0 to 65535 inclusive</returns>
    public ushort ReadUnsignedShort() => BitConverter.ToUInt16(ReadBytes(2));

    /// <returns>Value from -2147483648 to 2147483647 inclusive</returns>
    public int ReadInt() => BitConverter.ToInt32(ReadBytes(4));

    /// <returns>Value from -9223372036854775808 to 9223372036854775807 inclusive</returns>
    public long ReadLong() => BitConverter.ToInt64(ReadBytes(8));

    public float ReadFloat() => BitConverter.ToSingle(ReadBytes(4));

    public double ReadDouble() => BitConverter.ToDouble(ReadBytes(8));

    public Guid ReadUUID() => new Guid(ReadBytes(16));

    public int ReadVarInt()
    {
        int numRead = 0;
        int result = 0;
        byte read;
        do
        {
            read = ReadByte();
            int value = read & 0b01111111;
            result |= value << 7 * numRead;

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
            result |= value << 7 * numRead;

            numRead++;
            if (numRead > 10)
            {
                throw new Exception("VarLong is too big");
            }
        } while ((read & 0b10000000) != 0);

        return result;
    }

    public Vec3i ReadPosition()
    {
        long value = ReadLong();
        int x = (int)(value >> 38);
        int y = (int)(value >> 26 & 0xFFF);
        int z = (int)(value << 38 >> 38); // sign-extend
        return new Vec3i(x, y, z);
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

    public void Dispose() => stream.Dispose();
}
