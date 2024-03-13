using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using Vector2 = ShooterNetwork.Vector2;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private PlayerAnimatorController playerAnimatorController;
    [SerializeField] private PlayerAiming playerAiming;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform rifleWeaponHolder;
    [SerializeField] private Transform pistolWeaponHolder;
    [SerializeField] private AllWeapons weaponsList;
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
    private bool hasWeapon;

    private void Start()
    {
        if (weaponsList.weapons.Count < 1)
        {
            hasWeapon = false;
            return;
        }

        hasWeapon = true;
        EquipWeapon(weaponsList.weapons[0]);
        currentWeaponIndex = 0;
    }

    private void Update()
    {
        if(!hasWeapon) return;
        if (isFiring)
        {
            Shoot();
        }
    }


    private void EquipWeapon(Weapon weapon)
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            canFire = true;
            isReloading = false;
        }

        if (currentWeapon != null)
        {
            UnEquipWeapon();
        }

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

        WeaponConfig weaponConfig = weapon.GetWeaponConfig();
        reload1 = weaponConfig.reloadSound1;
        reload2 = weaponConfig.reloadSound2;
        reload3 = weaponConfig.reloadSound3;


        currentWeapon = clonedWeapon;

        currentWeapon.OnFireBullet += OnFireBullet;


        GameplayHUD.Instance.UpdateMaxAmmo(currentWeapon.GetWeaponConfig().maxAmmo);
        GameplayHUD.Instance.SetWeaponName(currentWeapon.GetWeaponConfig().weaponName);


        playerAnimatorController.SetAnimatorController(weaponAnimationType);

        SendEquipWeaponPacket(currentWeapon.ID);
    }

    private void OnFireBullet(Vector3 recoilOffset)
    {
        SendFireBulletPacket(recoilOffset);
        GameplayHUD.Instance.UpdateAmmo(currentWeapon.GetAmmo());
    }

    private void UnEquipWeapon()
    {
        currentWeapon.OnFireBullet -= SendFireBulletPacket;
        Destroy(currentWeapon.gameObject);
    }

    private void SendFireBulletPacket(Vector3 recoilOffset)
    {
        Vector2 rec = new Vector2(recoilOffset.x, recoilOffset.z);
        SendFireBulletPacket(rec);
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

    public void StartFiring()
    {
        isFiring = true;
        if (!playerAiming.IsAiming)
        {
            SendAimPacket(true);
        }
    }

    public void StopFiring()
    {
        isFiring = false;
        currentWeapon.StopFiring();
        if (!playerAiming.IsAiming)
        {
            SendAimPacket(false);
        }
    }

    private void Reload()
    {
        if (currentWeapon == null || weaponsList.weapons.Count == 1) return;
        if (isReloading || currentWeapon.IsAmmoFull()) return;

        playerAnimatorController.PlayReloadAnimation(currentWeapon.GetReloadTime());

        reloadCoroutine = StartCoroutine(Reloading());

        SendReloadPacket();
    }

    private IEnumerator Reloading()
    {
        isReloading = true;
        canFire = false;
        yield return new WaitForSeconds(currentWeapon.ReloadTime);
        currentWeapon.Reload();
        GameplayHUD.Instance.UpdateAmmo(currentWeapon.GetAmmo());
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

    private void NextWeapon()
    {
        if (currentWeaponIndex == weaponsList.weapons.Count - 1)
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex++;
        }

        EquipWeapon(weaponsList.weapons[currentWeaponIndex]);
    }

    private void PreviousWeapon()
    {
        if (currentWeaponIndex == 0)
        {
            currentWeaponIndex = weaponsList.weapons.Count - 1;
        }
        else
        {
            currentWeaponIndex--;
        }

        EquipWeapon(weaponsList.weapons[currentWeaponIndex]);
    }

    private void OnEnable()
    {
        inputReader.OnFireEvent += ReadFireButton;
        inputReader.OnReloadEvent += Reload;

        playerAnimatorController.OnReload1 += PlayReload1Sound;
        playerAnimatorController.OnReload2 += PlayReload2Sound;
        playerAnimatorController.OnReload3 += PlayReload3Sound;

        inputReader.OnNextWeaponEvent += NextWeapon;
        inputReader.OnPreviousWeaponEvent += PreviousWeapon;
    }

    private void OnDisable()
    {
        inputReader.OnFireEvent -= ReadFireButton;
        inputReader.OnReloadEvent -= Reload;

        playerAnimatorController.OnReload1 -= PlayReload1Sound;
        playerAnimatorController.OnReload2 -= PlayReload2Sound;
        playerAnimatorController.OnReload3 -= PlayReload3Sound;

        inputReader.OnNextWeaponEvent -= NextWeapon;
        inputReader.OnPreviousWeaponEvent -= PreviousWeapon;

        if (currentWeapon != null)
        {
            currentWeapon.OnFireBullet -= OnFireBullet;
        }
    }

    private void OnDestroy()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
    }
}