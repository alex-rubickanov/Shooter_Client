using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BurstWeapon : Weapon
{
    [SerializeField] private int bulletsInBurst;
    [SerializeField] private float timeBetweenBullet;
    public override void Shoot()
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
        
        StartCoroutine(FireBurst());
    }

    private IEnumerator FireBurst()
    {
        for (int i = 0; i < bulletsInBurst; i++)
        {
            FireBullet();
            yield return new WaitForSeconds(timeBetweenBullet);
        }
        
    }
}
