using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCatalogue : MainMenuSection
{
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    
    [SerializeField] private AllWeapons allWeapons;
    
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI fireModeText;
    [SerializeField] private TextMeshProUGUI fireRateText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI bulletSpeedText;
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] private TextMeshProUGUI recoilText;
    [SerializeField] private TextMeshProUGUI clipSizeText;
    [SerializeField] private TextMeshProUGUI reloadTimeText;

    [SerializeField] private RectTransform weaponHolder;
    
    private int currentWeaponIndex = 0;
    private new void Start()
    {
        base.Start();
        
        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);
        
        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex].GetWeaponConfig());
    }

    private void OnRightArrowClicked()
    {
        currentWeaponIndex++;
        if(currentWeaponIndex >= allWeapons.weapons.Count)
            currentWeaponIndex = 0;
        
        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex].GetWeaponConfig());
    }

    private void OnLeftArrowClicked()
    {
        currentWeaponIndex--;
        if(currentWeaponIndex < 0)
            currentWeaponIndex = allWeapons.weapons.Count - 1;
        
        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex].GetWeaponConfig());
    }

    private void SetWeaponWindow(WeaponConfig weaponConfig)
    {
        weaponNameText.text = weaponConfig.weaponName;
        fireModeText.text = "Fire Mode: " + weaponConfig.FireMode.ToString();
        fireRateText.text = "Fire Rate: " + weaponConfig.fireRate.ToString();
        damageText.text = "Damage: " + weaponConfig.damage.ToString();
        bulletSpeedText.text = "Bullet Speed: " + weaponConfig.bulletSpeed.ToString();
        weightText.text = "Weight: " + weaponConfig.weight.ToString();
        recoilText.text = "Recoil: " + weaponConfig.recoil.ToString();
        clipSizeText.text = "Clip Size: " + weaponConfig.maxAmmo.ToString();
        reloadTimeText.text = "Reload Time: " + weaponConfig.reloadTime.ToString() + " seconds";
        
        if(weaponHolder.childCount > 0)
            Destroy(weaponHolder.GetChild(0).gameObject);

        Instantiate(weaponConfig.weaponUI, weaponHolder);
    }
}
