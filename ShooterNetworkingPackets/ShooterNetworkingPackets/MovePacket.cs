namespace ShooterNetwork
{
    public class MovePacket : BasePacket
    {
        private Vector2 position;
        private Vector2 velocity;
        private float rotationY;
        public Vector2 Position => position;
        public Vector2 Velocity => velocity;
        public float RotationY => rotationY;

        public MovePacket()
        {
            position = new Vector2(0, 0);
            velocity = new Vector2(0, 0);
            rotationY = 0;
        }

        public MovePacket(Vector2 position, Vector2 velocity,float rotationY, IDataHolder dataHolder) : base(PacketType.Move, dataHolder)
        {
            this.position = position;
            this.velocity = velocity;
            this.rotationY = rotationY;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(position.X);
            sbw.Write(position.Y);
            sbw.Write(velocity.X);
            sbw.Write(velocity.Y);
            sbw.Write(rotationY);
            return EndSerialize();
        }

        public new MovePacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            position.X = dbr.ReadSingle();
            position.Y = dbr.ReadSingle();
            velocity.X = dbr.ReadSingle();
            velocity.Y = dbr.ReadSingle();
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
            X = x;
            Y = y;
        }
    }
}