using System;
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
    private int bulletsInClip;

    protected void Start()
    {
        bulletsInClip = weaponConfig.bulletsInClip;
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
            if (bulletsInClip == 0) return;
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
        bulletsInClip--;
        Vector3 recoilOffset = new Vector3(UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil), 0, UnityEngine.Random.Range(-weaponConfig.recoil, weaponConfig.recoil));
        Debug.DrawRay(muzzleTransform.position, muzzleTransform.forward * 30f + recoilOffset, Color.red, 2.0f);
    }

    public void StopFiring()
    {
        shotFired = false;
    }
}