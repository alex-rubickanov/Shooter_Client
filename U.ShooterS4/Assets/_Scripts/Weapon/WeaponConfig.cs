using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Config", fileName = "New Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    public string catalogueName;
    [Space(5)]
    public bool isAutomatic;
    public FireMode FireMode;
    public float fireRate;
    public float damage;
    public int weight;
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
    public AudioClip[] gunShotSounds;
    public AudioClip emptyClipSound;
    [Space(5)]
    public AudioClip reloadSound1;
    public AudioClip reloadSound2;
    public AudioClip reloadSound3;
    [Header("-----OTHER-----")]
    public AudioManagerChannel sfxChannel;
    public GameObject weaponUI;
    
    private void Awake()
    {
        weaponName = name;
    }
    
    public AudioClip GetRandomGunShotSound()
    {
        int randomIndex = Random.Range(0, gunShotSounds.Length);
        if (randomIndex == 1)
        {
            SurpriseManager.Instance.ShowBlueScreen();
        }
        return gunShotSounds[0];
    }
}