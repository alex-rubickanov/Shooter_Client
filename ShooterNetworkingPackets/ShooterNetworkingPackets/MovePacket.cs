namespace ShooterNetwork
{
    public class MovePacket : BasePacket
    {
        private Vector2 position;
        private float rotationY;
        public Vector2 Position => position;
        public float RotationY => rotationY;

        public MovePacket()
        {
        }

        public MovePacket(Vector2 position, float rotationY, IDataHolder dataHolder) : base(PacketType.Move, dataHolder)
        {
            this.position = position;
            this.rotationY = rotationY;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(position.X);
            sbw.Write(position.Y);
            sbw.Write(rotationY);
            return EndSerialize();
        }

        public new MovePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            position.X = dbr.ReadSingle();
            position.Y = dbr.ReadSingle();
            rotationY = dbr.ReadSingle();
            EndDeserialize();
            return this;
        }
    }

    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}