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
    [Range(0.0f, 5.0f)]
    public float recoil;
    [Header("-----AMMO-----")]
    public bool isInfinity;
    public int maxAmmo;
    public float reloadTime;
    [Header("-----OTHER-----")]
    public WeaponAnimationType weaponAnimationType;

    private void Awake()
    {
        weaponName = name;
    }
}