using UnityEngine;
using ShooterNetwork;
using Vector2 = ShooterNetwork.Vector2;

public class NetworkBehaviour : MonoBehaviour
{
    public void SendDebugLogPacket(string message)
    {
        DebugLogPacket debugLogPacket = new DebugLogPacket(message, Client.Instance.PlayerData);
        Client.Instance.SendPacket(debugLogPacket);
    }

    public void SendMovePacket(Vector2 position, Vector2 velocity, float rotationY)
    {
        MovePacket movePacket = new MovePacket(position, velocity, rotationY, Client.Instance.PlayerData);
        Client.Instance.SendPacket(movePacket);
    }
    
    public void SendAimPacket(bool isAiming)
    {
        AimPacket aimPacket = new AimPacket(isAiming, Client.Instance.PlayerData);
        Client.Instance.SendPacket(aimPacket);
    }
    
    public void SendEquipWeaponPacket(int weaponID)
    {
        EquipWeaponPacket equipWeaponPacket = new EquipWeaponPacket(weaponID, Client.Instance.PlayerData);
        Client.Instance.SendPacket(equipWeaponPacket);
    }
    
    public void SendFireBulletPacket(Vector2 recoilOffset)
    {
        FireBulletPacket fireBulletPacket = new FireBulletPacket(recoilOffset, Client.Instance.PlayerData);
        Client.Instance.SendPacket(fireBulletPacket);
    }
    
    public void SendReloadPacket()
    {
        ReloadPacket reloadPacket = new ReloadPacket(Client.Instance.PlayerData);
        Client.Instance.SendPacket(reloadPacket);
    }
    
    public void SendHitPacket(int hitSoundID)
    {
        HitPacket hitPacket = new HitPacket(hitSoundID, Client.Instance.PlayerData);
        Client.Instance.SendPacket(hitPacket);
    }
    
    public void SendDeathPacket(int deathSoundID)
    {
        DeathPacket deathPacket = new DeathPacket(deathSoundID, Client.Instance.PlayerData);
        Client.Instance.SendPacket(deathPacket);
    }
}