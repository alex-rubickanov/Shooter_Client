namespace ShooterNetwork
{
    public class PawnSpawnPacket : BasePacket
    {
        private Vector2 position;
        public Vector2 Position => position; 
        public PawnSpawnPacket()
        {
        }
        
        public PawnSpawnPacket(Vector2 position, PlayerData playerData) : base(PacketType.PlayerPawnSpawn, playerData)
        {
            this.position = position;
        }
        
        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(Position.X);
            sbw.Write(Position.Y);
            return EndSerialize();
        }
        
        public new PawnSpawnPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            position.X = dbr.ReadSingle();
            position.Y = dbr.ReadSingle();
            EndDeserialize();
            return (PawnSpawnPacket) base.Deserialize(buffer);
        }
    }
}