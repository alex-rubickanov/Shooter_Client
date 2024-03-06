namespace ShooterNetwork
{
    public class DebugLogPacket : BasePacket
    {
        public string Message { get; private set; }
    
        public DebugLogPacket()
        {
            Message = "";
        }

        public DebugLogPacket(string message, IDataHolder dataHolder) : base(PacketType.DebugLog, dataHolder)
        {
            Message = $"{message} from {dataHolder.Name}";
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(Message);
            return EndSerialize();
        }

        public new DebugLogPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            Message = dbr.ReadString();
            EndDeserialize();
            return this;
        }
    }
}