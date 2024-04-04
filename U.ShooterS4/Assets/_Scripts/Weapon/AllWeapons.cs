using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All Weapons", menuName = "Weapons/All Weapons")]
public class AllWeapons : ScriptableObject
{
    [SerializeField] public List<Weapon> weapons;
}
