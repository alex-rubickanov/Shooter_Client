namespace ShooterNetwork
{
    public class StartGamePacket : BasePacket
    {
        public StartGamePacket()
        {
        }
        
        public StartGamePacket(IDataHolder dataHolder) : base(PacketType.StartGame, dataHolder)
        {
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            return EndSerialize();
        }
        
        public new StartGamePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            EndDeserialize();
            return this;
        }
    }
}