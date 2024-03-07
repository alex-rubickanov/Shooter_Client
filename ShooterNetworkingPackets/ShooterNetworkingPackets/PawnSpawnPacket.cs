namespace ShooterNetwork
{
    public class PawnSpawnPacket : BasePacket
    {
        
        public PawnSpawnPacket()
        {
        }
        
        public PawnSpawnPacket(PlayerData playerData) : base(PacketType.PlayerPawnSpawn, playerData)
        {
        }
        
        public new PawnSpawnPacket Deserialize(byte[] buffer)
        {
            return (PawnSpawnPacket) base.Deserialize(buffer);
        }
    }
}