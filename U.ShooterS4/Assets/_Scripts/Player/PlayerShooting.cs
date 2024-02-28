using UnityEngine;

public class PlayerShooting : PlayerComponent
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Transform weaponHolder;

    private bool isFiring;

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
        Weapon clonedWeapon = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);

        currentWeapon = clonedWeapon;
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