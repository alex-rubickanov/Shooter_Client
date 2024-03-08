using System.IO;

namespace ShooterNetwork
{
    public enum PacketType
    {
        None = 0,
        DebugLog = 1,
        PlayerData = 2,
        AssignID = 3,
        PlayerPawnSpawn = 4,
        Move = 5,
        Aim = 6,
        EquipWeapon = 7
    }

    public class BasePacket
    {
        protected MemoryStream sms; // Serialize Memory Stream
        protected BinaryWriter sbw; // Serialize Binary Writer

        protected MemoryStream dms; // Deserialize Memory Stream 
        protected BinaryReader dbr; // Deserialize Binary Writer

        public IDataHolder DataHolder { get; private set; }
        public PacketType Type { get; private set; }
        public int PacketSize => packetSize;
        protected int packetSize;

        private static int currentBufferPosition;


        public BasePacket()
        {
            Type = PacketType.None;
        }

        public BasePacket(PacketType type, IDataHolder dataHolder)
        {
            Type = type;
            DataHolder = dataHolder;
        }

        public virtual byte[] Serialize()
        {
            BeginSerialize();
            return EndSerialize();
        }

        protected void BeginSerialize()
        {
            sms = new MemoryStream();
            sbw = new BinaryWriter(sms);

            sbw.Write((int)Type);
            sbw.Write(DataHolder.Name);
            sbw.Write(DataHolder.ID);
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
            EndDeserialize();
            return this;
        }

        protected void BeginDeserialize(byte[] buffer)
        {
            dms = new MemoryStream(buffer);
            //dms.Seek(currentBufferPosition, SeekOrigin.Begin);
            dbr = new BinaryReader(dms);

            Type = (PacketType)dbr.ReadInt32();
            DataHolder = new PlayerData(dbr.ReadString(), dbr.ReadString());
        }

        protected void EndDeserialize()
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
    }
}