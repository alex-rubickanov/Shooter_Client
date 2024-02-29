using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerShooting : PlayerComponent
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Transform rifleWeaponHolder;
    [SerializeField] private Transform pistolWeaponHolder;


    private bool canFire = true;
    private bool isFiring;
    private bool isReloading;

    public bool IsFiring => isFiring;

    private void Start()
    {
        if (currentWeapon != null)
        {
            EquipWeapon(currentWeapon);
        }
    }

    private void Update()
    {
        if (isFiring)
        {
            Shoot();
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        WeaponAnimationType weaponAnimationType = weapon.GetWeaponType();
        Weapon clonedWeapon = null;
        
        switch (weaponAnimationType)
        {
            case WeaponAnimationType.Pistol:
                clonedWeapon = Instantiate(weapon, pistolWeaponHolder.position, pistolWeaponHolder.rotation,
                    pistolWeaponHolder);

                break;
            
            case WeaponAnimationType.Rifle:
                clonedWeapon = Instantiate(weapon, rifleWeaponHolder.position, rifleWeaponHolder.rotation,
                    rifleWeaponHolder);

                break;
        }


        currentWeapon = clonedWeapon;

        playerManager.PlayerAnimatorController.SetAnimatorController(weaponAnimationType);
    }

    private void Shoot()
    {
        if (currentWeapon == null || !canFire) return;
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

    private void Reload()
    {
        if (currentWeapon == null) return;
        if (isReloading || currentWeapon.IsAmmoFull()) return;

        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        canFire = false;
        yield return new WaitForSeconds(currentWeapon.ReloadTime);
        currentWeapon.Reload();
        canFire = true;
        isReloading = false;
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
        inputReader.OnReloadEvent += Reload;
    }

    private void OnDisable()
    {
        inputReader.OnFireEvent -= ReadFireButton;
        inputReader.OnReloadEvent -= Reload;
    }
}