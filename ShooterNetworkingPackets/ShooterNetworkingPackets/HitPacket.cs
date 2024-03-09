namespace ShooterNetwork
{
    public class HitPacket : BasePacket
    {
        public int HitSoundID { get; private set; }
        
        public HitPacket()
        {
            HitSoundID = 0;
        }
        
        public HitPacket(int hitSoundID, IDataHolder dataHolder) : base(PacketType.Hit, dataHolder)
        {
            HitSoundID = hitSoundID;
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(HitSoundID);
            return EndSerialize();
        }
        
        public new HitPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            HitSoundID = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}