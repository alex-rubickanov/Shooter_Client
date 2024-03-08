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
    [SerializeField] private InputReader inputReader;
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

    private void Start()
    {
        EquipWeapon(weapons[0]);
        currentWeaponIndex = 0;
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
        if(currentWeapon != null)
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


        currentWeapon = clonedWeapon;

        currentWeapon.OnFireBullet += SendFireBulletPacket;

        playerAnimatorController.SetAnimatorController(weaponAnimationType);
        
        SendEquipWeaponPacket(currentWeaponIndex);
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
        if (currentWeaponIndex == weapons.Count - 1)
        {
            currentWeaponIndex = 0;
        }
        else
        {
            currentWeaponIndex++;
        }

        EquipWeapon(weapons[currentWeaponIndex]);
    }
    
    private void PreviousWeapon()
    {
        if (currentWeaponIndex == 0)
        {
            currentWeaponIndex = weapons.Count - 1;
        }
        else
        {
            currentWeaponIndex--;
        }

        EquipWeapon(weapons[currentWeaponIndex]);
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
    }

    private void OnDestroy()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
        }
    }
}