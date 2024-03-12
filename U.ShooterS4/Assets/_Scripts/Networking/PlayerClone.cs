using ShooterNetwork;
using UnityEngine;

public class PlayerClone : MonoBehaviour
{
    public PlayerData cloneData;
    
    private CloneMovement cloneMovement;
    private CloneAiming cloneAiming;
    private CloneShooting cloneShooting;
    private CloneHealth cloneHealth;
    [SerializeField] private CloneAnimatorController cloneAnimatorController;

    private void Awake()
    {
        cloneMovement = GetComponent<CloneMovement>();
        cloneAiming = GetComponent<CloneAiming>();
        cloneShooting = GetComponent<CloneShooting>();
        cloneHealth = GetComponent<CloneHealth>();
    }

    private void OnEnable()
    {
        Client.Instance.OnMovePacketReceived += MoveClone;
        Client.Instance.OnAimPacketReceived += AimClone;
        Client.Instance.OnEquipWeaponPacketReceived += EquipWeapon;
        Client.Instance.OnFireBulletPacketReceived += FireBullet;
        Client.Instance.OnReloadPacketReceived += Reload;
        Client.Instance.OnHitPacketReceived += Hit;
        Client.Instance.OnDeathPacketReceived += Death;
        Client.Instance.OnDancePacketReceived += Dance;
    }

    private void OnDisable()
    {
        Client.Instance.OnMovePacketReceived -= MoveClone;
        Client.Instance.OnAimPacketReceived -= AimClone;
        Client.Instance.OnEquipWeaponPacketReceived -= EquipWeapon;
        Client.Instance.OnFireBulletPacketReceived -= FireBullet;
        Client.Instance.OnReloadPacketReceived -= Reload;
        Client.Instance.OnHitPacketReceived -= Hit;
        Client.Instance.OnDeathPacketReceived -= Death;
        Client.Instance.OnDancePacketReceived -= Dance;
    }

    private void Dance(DancePacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneAnimatorController.PlayDance(packet.DanceID);
    }

    private void Death(DeathPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneHealth.Die(packet.DeathSoundID);
    }

    private void Hit(HitPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneHealth.TakeDamage(packet.HitSoundID);
    }

    private void Reload(ReloadPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneShooting.Reload();
    }

    private void FireBullet(FireBulletPacket packet)
    {
        if(packet.DataHolder.ID != cloneData.ID) return;
        cloneAiming.IsAiming = true;
        cloneShooting.FireBullet(packet.RecoilOffset, cloneData);
        
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