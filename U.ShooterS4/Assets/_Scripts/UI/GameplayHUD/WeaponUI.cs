using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI ammoText;
    private string maxAmmo;
    
    public void SetWeaponName(string weaponName)
    {
        weaponNameText.text = weaponName;
    }
    
    public void UpdateMaxAmmo(int maxAmmo)
    {
        this.maxAmmo = maxAmmo.ToString();
        UpdateAmmo(maxAmmo);
    }
    
    public void UpdateAmmo(int ammo)
    {
        ammoText.text = $"{ammo} / {maxAmmo}";
    }
}
