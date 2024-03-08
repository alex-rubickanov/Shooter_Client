namespace ShooterNetwork
{
    public class FireBulletPacket : BasePacket
    {
        private Vector2 recoilOffset;
        public Vector2 RecoilOffset => recoilOffset;
        
        public FireBulletPacket()
        {
        }
        
        public FireBulletPacket(Vector2 recoilOffset, IDataHolder dataHolder) : base(PacketType.FireBullet, dataHolder)
        {
            this.recoilOffset = recoilOffset;
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(recoilOffset.X);
            sbw.Write(recoilOffset.Y);
            return EndSerialize();
        }
        
        public new FireBulletPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            recoilOffset.X = dbr.ReadSingle();
            recoilOffset.Y = dbr.ReadSingle();
            EndDeserialize();
            return this;
        }
    }
}