namespace ShooterNetwork
{
    public class DeathPacket : BasePacket
    {
        public int KillerID { get; private set; }
        public int DeathSoundID { get; private set; }

        public DeathPacket()
        {
            KillerID = 0;
            DeathSoundID = 0;
        }

        public DeathPacket(int killerId, int deathSoundID, IDataHolder dataHolder) : base(PacketType.Death, dataHolder)
        {
            KillerID = killerId;
            DeathSoundID = deathSoundID;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(KillerID);
            sbw.Write(DeathSoundID);
            return EndSerialize();
        }

        public new DeathPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            KillerID = dbr.ReadInt32();
            DeathSoundID = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}