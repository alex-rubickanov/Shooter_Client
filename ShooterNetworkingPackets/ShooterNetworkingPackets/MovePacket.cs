namespace ShooterNetwork
{
    public class MovePacket : BasePacket
    {
        private Vector2 position;
        public Vector2 Position => position;

        public MovePacket()
        {
        }

        public MovePacket(Vector2 position, IDataHolder dataHolder) : base(PacketType.Move, dataHolder)
        {
            this.position = position;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(position.x);
            sbw.Write(position.y);
            return EndSerialize();
        }

        public new MovePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            position.x = dbr.ReadSingle();
            position.y = dbr.ReadSingle();
            EndDeserialize();
            return this;
        }
    }

    public struct Vector2
    {
        public float x;
        public float y;
        
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}