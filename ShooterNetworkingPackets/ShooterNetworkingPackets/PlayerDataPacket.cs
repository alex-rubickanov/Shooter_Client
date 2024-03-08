namespace ShooterNetwork
{
    public class PlayerDataPacket : BasePacket
    {
        public PlayerDataPacket()
        {
        }

        public PlayerDataPacket(PlayerData playerData) : base(PacketType.PlayerData, playerData)
        {
        }

        public new PlayerDataPacket Deserialize(byte[] buffer)
        {
            return (PlayerDataPacket)base.Deserialize(buffer);
        }
    }
}