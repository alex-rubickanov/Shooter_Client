using System.Collections;
using UnityEngine;

public class BurstWeapon : Weapon
{
    [SerializeField] private int bulletsInBurst;
    [SerializeField] private float timeBetweenBullet;

    public override void Shoot()
    {
        if (!CanFire()) return;
        
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
