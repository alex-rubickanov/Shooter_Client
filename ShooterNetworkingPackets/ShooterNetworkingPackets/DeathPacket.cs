namespace ShooterNetwork
{
    public class DeathPacket : BasePacket
    {
        public int DeathSoundID { get; private set; }

        public DeathPacket()
        {
            DeathSoundID = 0;
        }

        public DeathPacket(int deathSoundID, IDataHolder dataHolder) : base(PacketType.Death, dataHolder)
        {
            DeathSoundID = deathSoundID;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(DeathSoundID);
            return EndSerialize();
        }

        public new DeathPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            DeathSoundID = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}