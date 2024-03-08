namespace ShooterNetwork
{
    public class AimPacket : BasePacket
    {
        public bool IsAiming { get; private set; }

        public AimPacket()
        {
        }

        public AimPacket(bool isAiming, IDataHolder dataHolder) : base(PacketType.Aim, dataHolder)
        {
            IsAiming = isAiming;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(IsAiming);
            return EndSerialize();
        }

        public new AimPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            IsAiming = dbr.ReadBoolean();
            EndDeserialize();
            return this;
        }
    }
}