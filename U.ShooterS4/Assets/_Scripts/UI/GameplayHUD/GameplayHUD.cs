using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    public static GameplayHUD Instance;

    [SerializeField] private WeaponUI weaponUI;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        if (Client.Instance.showStartGameMenu)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SetWeaponName(string weaponName)
    {
        weaponUI.SetWeaponName(weaponName);
    }

    public void UpdateMaxAmmo(int maxAmmo)
    {
        weaponUI.UpdateMaxAmmo(maxAmmo);
    }

    public void UpdateAmmo(int ammo)
    {
        weaponUI.UpdateAmmo(ammo);
    }
}