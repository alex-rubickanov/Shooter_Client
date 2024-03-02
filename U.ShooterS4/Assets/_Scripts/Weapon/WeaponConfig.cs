using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Config", fileName = "New Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    [Space(5)]
    public bool isAutomatic;
    public float fireRate;
    public float damage;
    public float bulletSpeed;
    public float recoil;
    [Header("-----AMMO-----")]
    public GameObject bulletPrefab;
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