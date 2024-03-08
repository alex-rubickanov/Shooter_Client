using ShooterNetwork;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    private PlayerData cloneData;
    private CloneMovement cloneMovement;
    private CloneAiming cloneAiming;
    private CloneShooting cloneShooting;

    private void Awake()
    {
        cloneMovement = GetComponent<CloneMovement>();
        cloneAiming = GetComponent<CloneAiming>();
        cloneShooting = GetComponent<CloneShooting>();
    }

    private void OnEnable()
    {
        Client.Instance.OnMovePacketReceived += MoveClone;
        Client.Instance.OnAimPacketReceived += AimClone;
        Client.Instance.OnEquipWeaponPacketReceived += EquipWeapon;
        Client.Instance.OnFireBulletPacketReceived += FireBullet;
        Client.Instance.OnReloadPacketReceived += Reload;
    }

    private void OnDisable()
    {
        Client.Instance.OnMovePacketReceived -= MoveClone;
        Client.Instance.OnAimPacketReceived -= AimClone;
        Client.Instance.OnEquipWeaponPacketReceived -= EquipWeapon;
        Client.Instance.OnFireBulletPacketReceived -= FireBullet;
        Client.Instance.OnReloadPacketReceived -= Reload;
    }
    
    private void Reload(ReloadPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneShooting.Reload();
    }

    private void FireBullet(FireBulletPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneShooting.FireBullet(packet.RecoilOffset);
    }

    private void EquipWeapon(EquipWeaponPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneShooting.EquipWeapon(packet.WeaponID);
    }

    private void AimClone(AimPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneAiming.IsAiming = packet.IsAiming;
    }

    private void MoveClone(MovePacket packet)
    {
        if (packet.DataHolder.ID != cloneData.ID) return;
        cloneMovement.Move(new UnityEngine.Vector2(packet.Position.X, packet.Position.Y),
            new UnityEngine.Vector2(packet.Velocity.X, packet.Velocity.Y));
        cloneMovement.Rotate(packet.RotationY);
    }

    public void SetCloneData(PlayerData data)
    {
        cloneData = data;
    }
}