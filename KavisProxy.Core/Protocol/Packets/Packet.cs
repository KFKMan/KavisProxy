namespace KavisProxy.Core.Protocol;

//Clientbound = Which CLIENT GET
//Serverbound = Which SERVER GET

public interface Packet
{
    public int PacketID { get; }
}

public interface Packet<T> : Packet where T : class
{
    T Read(IByteBuffer buffer);

    void Write(IByteBuffer buffer, T value);
}