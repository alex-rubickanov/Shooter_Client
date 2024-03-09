namespace ShooterNetwork
{
    public class EquipWeaponPacket : BasePacket
    {
        public int WeaponID { get; private set; }

        public EquipWeaponPacket()
        {
            WeaponID = 0;
        }

        public EquipWeaponPacket(int weaponID, IDataHolder dataHolder) : base(PacketType.EquipWeapon, dataHolder)
        {
            WeaponID = weaponID;
        }

        public override byte[] Serialize()
        {
            BeginSerialize();
            sbw.Write(WeaponID);
            return EndSerialize();
        }

        public new EquipWeaponPacket Deserialize(byte[] buffer)
        {
            BeginDeserialize(buffer);
            WeaponID = dbr.ReadInt32();
            EndDeserialize();
            return this;
        }
    }
}