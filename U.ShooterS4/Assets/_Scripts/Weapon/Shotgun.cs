using ShooterNetwork;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private Transform[] bulletSpawners;

    protected override void FireBullet()
    {
        timer = 0.0f;
        ammo--;
        Vector3 recoilOffset = Vector3.zero;

        for (int i = 0; i < bulletSpawners.Length; i++)
        {
            var bullet = Instantiate(weaponConfig.bulletPrefab, bulletSpawners[i].position, Quaternion.identity);
            bullet.Initialize(owner, Client.Instance.PlayerData, weaponConfig.damage);

            bullet.GetComponent<Rigidbody>().AddForce(
                bulletSpawners[i].forward * weaponConfig.bulletSpeed + recoilOffset,
                ForceMode.Impulse);
            Destroy(bullet.gameObject, 1.5f);
        }


        var muzzleFlash = Instantiate(weaponConfig.muzzleFlash, muzzleTransform.position, muzzleTransform.rotation);
        Destroy(muzzleFlash.gameObject, 2.0f);

        weaponConfig.sfxChannel.RaiseEvent(weaponConfig.GetRandomGunShotSound(), muzzleTransform.position);

        if (ammo == 0) weaponConfig.sfxChannel.RaiseEvent(weaponConfig.emptyClipSound, muzzleTransform.position);

        InvokeFireBulletEvent(recoilOffset);
    }
    
    public override void FireBulletClone(Vector3 recoilOffset, PlayerData cloneData)
    {
        for (int i = 0; i < bulletSpawners.Length; i++)
        {
            var bullet = Instantiate(weaponConfig.bulletPrefab, bulletSpawners[i].position, Quaternion.identity);
            bullet.Initialize(owner, cloneData, weaponConfig.damage);

            bullet.GetComponent<Rigidbody>().AddForce(
                bulletSpawners[i].forward * weaponConfig.bulletSpeed + recoilOffset,
                ForceMode.Impulse);
            Destroy(bullet.gameObject, 1.5f);
        }

        var muzzleFlash = Instantiate(weaponConfig.muzzleFlash, muzzleTransform.position, muzzleTransform.rotation);

        weaponConfig.sfxChannel.RaiseEvent(weaponConfig.GetRandomGunShotSound(), muzzleTransform.position);


        Destroy(muzzleFlash.gameObject, 2.0f);
    }
}