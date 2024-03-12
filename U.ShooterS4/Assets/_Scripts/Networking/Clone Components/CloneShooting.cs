using System.Collections;
using ShooterNetwork;
using UnityEngine;
using Vector2 = ShooterNetwork.Vector2;

public class CloneShooting : MonoBehaviour
{
    [SerializeField] private CloneAnimatorController playerAnimatorController;
    [SerializeField] private Transform rifleWeaponHolder;
    [SerializeField] private Transform pistolWeaponHolder;
    [SerializeField] private AllWeapons weaponsList;
    [SerializeField] private AudioManagerChannel audioManagerChannel;

    [SerializeField] public AudioClip reload1;
    [SerializeField] public AudioClip reload2;
    [SerializeField] public AudioClip reload3;

    private Weapon currentWeapon;
    private int currentWeaponIndex;
    private Coroutine reloadCoroutine;


    public void EquipWeapon(int weaponID)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        Weapon weapon = FindPickedWeapon(weaponID);


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
        if (reload1 != weaponConfig.reloadSound1)
        {
            reload1 = weaponConfig.reloadSound1;
            reload2 = weaponConfig.reloadSound2;
            reload3 = weaponConfig.reloadSound3;
        }

        currentWeapon = clonedWeapon;

        playerAnimatorController.SetAnimatorController(weaponAnimationType);
    }

    private Weapon FindPickedWeapon(int ID)
    {
        Weapon pickedWeapon = null;
        foreach (var weapon in weaponsList.weapons)
        {
            if (weapon.ID == ID)
            {
                pickedWeapon = weapon;
                return pickedWeapon;
            }
        }

        return null;
    }

    public void FireBullet(Vector2 recoilOffset, PlayerData cloneData)
    {
        Vector3 rec = new Vector3(recoilOffset.X, 0, recoilOffset.Y);
        currentWeapon.FireBulletClone(rec, cloneData);
    }

    public void Reload()
    {
        playerAnimatorController.PlayReloadAnimation(currentWeapon.GetReloadTime());

        reloadCoroutine = StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(currentWeapon.ReloadTime);
        currentWeapon.Reload();
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