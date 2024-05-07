using UnityEngine;

public class WaterGun : Weapon
{
    [SerializeField] private AudioSource audioSource;

    protected override void FireBullet()
    {
        base.FireBullet();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    protected override void PlayShotSound()
    {
    }

    public override void StopFiring()
    {
        base.StopFiring();

        audioSource.Stop();
    }
}