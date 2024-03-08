namespace ShooterNetwork
{
    public class ReloadPacket : BasePacket
    {
        public ReloadPacket()
        {
        }

        public ReloadPacket(IDataHolder dataHolder) : base(PacketType.Reload, dataHolder)
        {
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            return EndSerialize();
        }

        public new ReloadPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            EndDeserialize();
            return this;
        }
    }
}