namespace ShooterNetwork
{
    public class ConnectionPacket : BasePacket
    {
        public int PlayersAmount { get; private set; }
        
        public ConnectionPacket()
        {
            
        }
        
        public ConnectionPacket(int playersAmount, IDataHolder dataHolder) : base(PacketType.Connection, dataHolder)
        {
            PlayersAmount = playersAmount;
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(PlayersAmount);
            return EndSerialize();
        }
        
        public new ConnectionPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            PlayersAmount = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}