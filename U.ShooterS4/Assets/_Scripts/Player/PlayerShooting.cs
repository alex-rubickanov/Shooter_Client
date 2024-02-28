using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private BaseWeapon currentWeapon;
    [SerializeField] private Transform weaponHolder;

    private bool isFiring;
    private void Start()
    {
        if (currentWeapon != null)
        {
            //EquipWeapon(currentWeapon);
        }
    }

    private void Update()
    {
        if(isFiring)
        {
            Shoot();
        }
    }

    private void EquipWeapon(BaseWeapon weapon)
    {
        currentWeapon = weapon;
        GameObject weaponPrefab = currentWeapon.GetWeaponPrefab();
        Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
    }

    private void Shoot()
    {
        if (currentWeapon == null) return;
        currentWeapon.Shoot();
    }

    public void StartFiring()
    {
        isFiring = true;
    }

    public void StopFiring()
    {
        isFiring = false;
        currentWeapon.StopFiring();
    }

    private void ReadFireButton(bool isFiring)
    {
        if (isFiring)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    private void OnEnable()
    {
        inputReader.OnFireEvent += ReadFireButton;
    }

    private void OnDisable()
    {
        inputReader.OnFireEvent -= ReadFireButton;
    }
}