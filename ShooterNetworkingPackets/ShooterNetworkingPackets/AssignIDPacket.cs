namespace ShooterNetwork
{
    public class AssignIDPacket : BasePacket
    {
        public string ID { get; private set; }

        public AssignIDPacket()
        {
            ID = "0";
        }

        public AssignIDPacket(string id, ServerData serverData) : base(PacketType.AssignID, serverData)
        {
            ID = id;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(ID);
            return EndSerialize();
        }

        public new AssignIDPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            ID = dbr.ReadString();
            EndDeserialize();
            return this;
        }
    }
}