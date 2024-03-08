using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneShooting : MonoBehaviour
{
    [SerializeField] private CloneAnimatorController playerAnimatorController;
    [SerializeField] private Transform rifleWeaponHolder;
    [SerializeField] private Transform pistolWeaponHolder;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private AudioManagerChannel audioManagerChannel;

    [SerializeField] public AudioClip reload1;
    [SerializeField] public AudioClip reload2;
    [SerializeField] public AudioClip reload3;

    private Weapon currentWeapon;
    private int currentWeaponIndex;
    private bool canFire = true;
    private bool isFiring;
    private bool isReloading;
    private Coroutine reloadCoroutine;

    public bool IsFiring => isFiring;

    public void EquipWeapon(int weaponID)
    {
        Weapon weapon = weapons[weaponID];
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

        playerAnimatorController.SetAnimatorController(weaponAnimationType);
    }

    private void Shoot()
    {
        if (currentWeapon == null || !canFire) return;
        if (currentWeapon.GetAmmo() == 0)
        {
            audioManagerChannel.RaiseEvent(currentWeapon.GetEmptyClipSound(), transform.position);
            isFiring = false;
        }

        currentWeapon.Shoot();
    }

    private void Reload()
    {
        if (currentWeapon == null) return;
        if (isReloading || currentWeapon.IsAmmoFull()) return;

        playerAnimatorController.PlayReloadAnimation(currentWeapon.GetReloadTime());

        reloadCoroutine = StartCoroutine(Reloading());
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

    private void PlayReload1Sound()
    {
        audioManagerChannel.RaiseEvent(reload1, currentWeapon.transform.position);
    }

    private void PlayReload2Sound()
    {
        audioManagerChannel.RaiseEvent(reload2, currentWeapon.transform.position);
    }

    private void PlayReload3Sound()
    {
        audioManagerChannel.RaiseEvent(reload3, currentWeapon.transform.position);
    }


    private void OnEnable()
    {
        playerAnimatorController.OnReload1 += PlayReload1Sound;
        playerAnimatorController.OnReload2 += PlayReload2Sound;
        playerAnimatorController.OnReload3 += PlayReload3Sound;
    }

    private void OnDisable()
    {
        playerAnimatorController.OnReload1 -= PlayReload1Sound;
        playerAnimatorController.OnReload2 -= PlayReload2Sound;
        playerAnimatorController.OnReload3 -= PlayReload3Sound;
    }

    private void OnDestroy()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
    }
}