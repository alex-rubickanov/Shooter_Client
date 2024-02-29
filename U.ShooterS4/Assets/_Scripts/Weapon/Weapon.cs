using System;
using UnityEditor.Animations;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private Transform muzzleTransform;

    private Ray ray;
    private RaycastHit hitInfo;

    private float timer;
    private bool shotFired = false;

    private int maxClips;
    private int ammo;
    
    public float ReloadTime => weaponConfig.reloadTime;

    protected void Start()
    {
        ammo = weaponConfig.maxAmmo;
        timer = weaponConfig.fireRate;
    }

    protected void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (!weaponConfig.isInfinity)
        {
            if (ammo == 0) return;
        }

        if (!weaponConfig.isAutomatic)
        {
            if (shotFired)
            {
                return;
            }

            shotFired = true;
        }

        if (timer < weaponConfig.fireRate) return;
        timer = 0.0f;
        ammo--;
        Vector3 recoilOffset = new Vector3(UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil), 0, UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil));
        Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * 30f + recoilOffset, Color.red, 2.0f);
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
    
    public WeaponAnimationType GetWeaponType()
    {
        return weaponConfig.weaponAnimationType;
    }
}