using System.IO;

namespace ShooterNetwork
{
    public enum PacketType
    {
        None = 0,
        DebugLog = 1,
        AssignID = 2,
        PlayerPawnSpawn = 3,
        Move = 4,
        Aim = 5,
        EquipWeapon = 6,
        FireBullet = 7,
        Reload = 8,
        Hit = 9,
        Death = 10,
        StartGame = 11,
        Dance = 12
    }

    public class BasePacket
    {
        protected MemoryStream sms; // Serialize Memory Stream
        protected BinaryWriter sbw; // Serialize Binary Writer

        protected MemoryStream dms; // Deserialize Memory Stream 
        protected BinaryReader dbr; // Deserialize Binary Writer

        public IDataHolder DataHolder { get; private set; }
        public PacketType Type { get; private set; }
        public int PacketSize { get; private set; }

        private static int currentBufferPosition = 0;
        
        private const int TYPE_SIZE = 4;

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
            PacketSize = (int)sms.Length + TYPE_SIZE;
            sbw.Write(PacketSize);
            return sms.ToArray();
        }

        public BasePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            //EndDeserialize();
            return this;
        }

        protected void BeginDeserialize(byte[] buffer)
        {
            dms = new MemoryStream(buffer);
            dms.Seek(currentBufferPosition, SeekOrigin.Begin);
            dbr = new BinaryReader(dms);

            Type = (PacketType)dbr.ReadInt32();
            DataHolder = new PlayerData(dbr.ReadString(), dbr.ReadString());
        }

        protected void EndDeserialize()
        {
            PacketSize = dbr.ReadInt32();
            currentBufferPosition += PacketSize;
        }

        public static bool DataRemainingInBuffer(int bufferSize)
        {
            if (currentBufferPosition < bufferSize)
                return true;

            currentBufferPosition = 0;
            return false;
        }

        public static void Reset()
        {
            currentBufferPosition = 0;
        }
    }
}