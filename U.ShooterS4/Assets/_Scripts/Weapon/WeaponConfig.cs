using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Config", fileName = "New Weapon Config")]
public class WeaponConfig : ScriptableObject
{
    public bool isAutomatic;
    public float fireRate;

}

