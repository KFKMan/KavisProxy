namespace KavisProxy.Core.Protocol.Packets;

public class TimeUpdate : Packet<TimeUpdateData>
{
    public int PacketID => 0x03;

    public TimeUpdateData Read(IByteBuffer buffer)
    {
        return new TimeUpdateData(buffer.ReadLong(), buffer.ReadLong());
    }

    public void Write(IByteBuffer buffer, TimeUpdateData value)
    {
        buffer.WriteLong(value.WorldAge);
        buffer.WriteLong(value.TimeOfDay);
    }
}

/// <summary>
/// 
/// </summary>
/// <param name="WorldAge">it's Long not VarLong | In ticks; not changed by server commands</param>
/// <param name="TimeOfDay">it's Long not VarLong | The world (or region) time, in ticks. If negative the sun will stop moving at the Math.abs of the time ??</param>
public record TimeUpdateData(long WorldAge, long TimeOfDay);
