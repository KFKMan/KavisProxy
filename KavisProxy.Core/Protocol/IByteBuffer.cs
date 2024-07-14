namespace KavisProxy.Core.Protocol;

public interface IByteBuffer
{
    int GetReadedCount();
    byte ReadAngle();
    bool ReadBool();
    byte ReadByte(bool AddToIndex = true);
    sbyte ReadByteInt();
    byte[] ReadBytes(int count, bool AddToIndex = true);
    byte ReadByteUnsigned();
    double ReadDouble();
    float ReadFloat();
    int ReadInt();
    long ReadLong();
    Vec3i ReadPosition();
    short ReadShort();
    string ReadString();
    ushort ReadUnsignedShort();
    Guid ReadUUID();
    int ReadVarInt();
    long ReadVarLong();
    byte[] ToArray();
    byte[] ToArrayWithCurrentIndex();
    void WriteAngle(byte angle);
    void WriteBool(bool value);
    void WriteByte(byte data);
    void WriteByteInt(sbyte value);
    void WriteBytes(byte[] data);
    void WriteByteUnsigned(byte value);
    void WriteDouble(double value);
    void WriteFloat(float value);
    void WriteInt(int value);
    void WriteLong(long value);
    void WritePosition(Vec3i position);
    void WriteShort(short value);
    void WriteString(string value);
    void WriteUnsignedShort(ushort value);
    void WriteUUID(Guid value);
    void WriteVarInt(int value);
    void WriteVarLong(long value);
}