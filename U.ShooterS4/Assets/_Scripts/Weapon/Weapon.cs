using System;
using ShooterNetwork;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected PlayerPawn owner;

    [SerializeField] protected WeaponConfig weaponConfig;
    [SerializeField] protected Transform muzzleTransform;

    protected Ray ray;
    protected RaycastHit hitInfo;

    protected float timer;
    protected bool shotFired = false;

    protected int ammo;

    public float ReloadTime => weaponConfig.reloadTime;

    public event Action<Vector3> OnFireBullet;

    protected void Start()
    {
        ammo = weaponConfig.maxAmmo;
        timer = weaponConfig.fireRate;

        owner = GetComponentInParent<PlayerPawn>();
    }

    protected void Update()
    {
        timer += Time.deltaTime;
    }

    public virtual void Shoot()
    {
        if (!CanFire()) return;
        FireBullet();
    }

    protected virtual bool CanFire()
    {
        if (ammo <= 0 && !weaponConfig.isInfinity)
        {
            return false;
        }

        if (!weaponConfig.isAutomatic)
        {
            if (shotFired)
            {
                return false;
            }

            shotFired = true;
        }

        if (timer < weaponConfig.fireRate) return false;
        return true;
    }

    protected virtual void FireBullet()
    {
        timer = 0.0f;
        ammo--;
        Vector3 recoilOffset = new Vector3(UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil), 0,
            UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil));

        var bullet = Instantiate(weaponConfig.bulletPrefab, muzzleTransform.position, Quaternion.identity);
        bullet.Initialize(owner, Client.Instance.PlayerData, weaponConfig.damage);

        bullet.GetComponent<Rigidbody>().AddForce(muzzleTransform.forward * weaponConfig.bulletSpeed + recoilOffset,
            ForceMode.Impulse);

        var muzzleFlash = Instantiate(weaponConfig.muzzleFlash, muzzleTransform.position, muzzleTransform.rotation);
        Destroy(muzzleFlash.gameObject, 2.0f);

        weaponConfig.sfxChannel.RaiseEvent(weaponConfig.gunShotSound, muzzleTransform.position);

        if (ammo == 0) weaponConfig.sfxChannel.RaiseEvent(weaponConfig.emptyClipSound, muzzleTransform.position);

        InvokeFireBulletEvent(recoilOffset);

        Destroy(bullet.gameObject, 1.5f);
    }

    public virtual void FireBulletClone(Vector3 recoilOffset, PlayerData cloneData)
    {
        var bullet = Instantiate(weaponConfig.bulletPrefab, muzzleTransform.position, Quaternion.identity);
        bullet.Initialize(owner, cloneData, weaponConfig.damage);

        bullet.GetComponent<Rigidbody>().AddForce(muzzleTransform.forward * weaponConfig.bulletSpeed + recoilOffset,
            ForceMode.Impulse);

        var muzzleFlash = Instantiate(weaponConfig.muzzleFlash, muzzleTransform.position, muzzleTransform.rotation);

        weaponConfig.sfxChannel.RaiseEvent(weaponConfig.gunShotSound, muzzleTransform.position);


        Destroy(muzzleFlash.gameObject, 2.0f);
        Destroy(bullet.gameObject, 1.5f);
    }

    protected void InvokeFireBulletEvent(Vector3 recoilOffset)
    {
        OnFireBullet?.Invoke(recoilOffset);
    }

    protected void InvokeEmptyFireBulletEvent()
    {
        OnFireBullet?.Invoke(Vector3.zero);
    }

    public void StopFiring()
    {
        shotFired = false;
    }

    public void Reload()
    {
        ammo = weaponConfig.maxAmmo;
    }

    public bool IsAmmoFull()
    {
        return ammo == weaponConfig.maxAmmo;
    }

    public int GetAmmo()
    {
        return ammo;
    }

    public WeaponAnimationType GetWeaponType()
    {
        return weaponConfig.weaponAnimationType;
    }

    public WeaponConfig GetWeaponConfig()
    {
        return weaponConfig;
    }

    public Transform GetMuzzleTransform()
    {
        return muzzleTransform;
    }

    public AudioClip GetEmptyClipSound()
    {
        return weaponConfig.emptyClipSound;
    }

    public float GetReloadTime()
    {
        return weaponConfig.reloadTime;
    }
}