using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Config", fileName = "New Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    [Space(5)]
    public bool isAutomatic;
    public float fireRate;
    public float damage;
    [Range(50, 130)] public float bulletSpeed;
    public float recoil;
    [Header("-----AMMO-----")]
    public Bullet bulletPrefab;
    public bool isInfinity;
    public int maxAmmo;
    public float reloadTime;
    [Header("-----VFX AND SFX-----")]
    public WeaponAnimationType weaponAnimationType;
    public ParticleSystem muzzleFlash;
    public AudioClip gunShotSound;
    public AudioClip emptyClipSound;
    [Header("-----OTHER-----")]
    public AudioManagerChannel sfxChannel;
    
    private void Awake()
    {
        weaponName = name;
    }
}