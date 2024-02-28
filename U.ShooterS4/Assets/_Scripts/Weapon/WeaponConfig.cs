using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Config", fileName = "New Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    [Space(5)]
    public bool isAutomatic;
    public float fireRate;
    [Header("-----AMMO-----")] 
    public bool isInfinity;
    public int maxClips;
    public int bulletsInClip;

    private void Awake()
    {
        weaponName = name;
    }
}

