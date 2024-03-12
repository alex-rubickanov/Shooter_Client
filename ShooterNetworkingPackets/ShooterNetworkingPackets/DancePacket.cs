namespace ShooterNetwork
{
    public class DancePacket : BasePacket
    {
        public int DanceID { get; private set; }
        
        public DancePacket()
        {
            DanceID = 0;
        }
        
        public DancePacket(int danceID, IDataHolder dataHolder) : base(PacketType.Dance, dataHolder)
        {
            DanceID = danceID;
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(DanceID);
            return EndSerialize();
        }
        
        public new DancePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            DanceID = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}