using System.IO;

public class BasePacket
{
    protected MemoryStream sms; // Serialize Memory Stream
    protected BinaryWriter sbw; // Serialize Binary Writer

    protected MemoryStream dms; // Deserialize Memory Stream 
    protected BinaryReader dbr; // Deserialize Binary Writer

    protected int packetSize;

    public PacketType Type { get; private set; }
    public int PacketSize => packetSize;

    private static int currentBufferPosition;


    public BasePacket()
    {
        Type = PacketType.None;
    }

    public BasePacket(PacketType type)
    {
        Type = type;
    }

    protected byte[] Serialize()
    {
        BeginSerialize();

        sbw.Write((int)Type);

        return EndSerialize();
    }

    protected void BeginSerialize()
    {
        sms = new MemoryStream();
        sbw = new BinaryWriter(sms);
    }

    protected byte[] EndSerialize()
    {
        packetSize = (int)sms.Length + 4;
        sbw.Write(packetSize);
        return sms.ToArray();
    }

    public BasePacket Deserialize(byte[] buffer)
    {
        BeginDeserialize(buffer);
        return this;
    }

    protected void BeginDeserialize(byte[] buffer)
    {
        dms = new MemoryStream(buffer);
        dms.Seek(currentBufferPosition, SeekOrigin.Begin);
        dbr = new BinaryReader(dms);

        Type = (PacketType)dbr.ReadInt32();
    }


    protected void EndDeserialize(ref byte[] buffer)
    {
        packetSize = dbr.ReadInt32();
        currentBufferPosition += packetSize;
    }

    public static bool DataRemainingInBuffer(int bufferSize)
    {
        if (currentBufferPosition < bufferSize)
            return true;

        currentBufferPosition = 0;
        return false;
    }

    public void Reset()
    {
        currentBufferPosition = 0;
    }
}