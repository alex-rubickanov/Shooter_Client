using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCatalogue : MainMenuSection
{
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private Button pickButton;

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
    private WeaponConfig currentWeapon;

    [SerializeField] private AllWeapons pickedWeapons;
    [SerializeField] private TextMeshProUGUI pickedWeapon1;
    [SerializeField] private TextMeshProUGUI pickedWeapon2;

    [SerializeField] private GameObject congratsMessage;
    [SerializeField] private TextMeshProUGUI killsToUnlockText;

    private new void Start()
    {
        base.Start();

        leftArrow.onClick.AddListener(OnLeftArrowClicked);
        rightArrow.onClick.AddListener(OnRightArrowClicked);
        pickButton.onClick.AddListener(OnPickButtonClicked);

        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex]);
        UpdatePickedWeapons();
    }

    private void OnPickButtonClicked()
    {
        if (pickedWeapons.weapons.Contains(allWeapons.weapons[currentWeaponIndex]))
            return;

        PickWeapon(allWeapons.weapons[currentWeaponIndex]);
    }

    private void PickWeapon(Weapon weapon)
    {
        if (pickedWeapons.weapons.Count >= 2)
        {
            pickedWeapons.weapons.RemoveAt(0);
        }

        pickedWeapons.weapons.Add(weapon);
        pickButton.interactable = false;
        UpdatePickedWeapons();
    }

    private void OnRightArrowClicked()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= allWeapons.weapons.Count)
            currentWeaponIndex = 0;

        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex]);
    }

    private void OnLeftArrowClicked()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 0)
            currentWeaponIndex = allWeapons.weapons.Count - 1;

        SetWeaponWindow(allWeapons.weapons[currentWeaponIndex]);
    }

    private void SetWeaponWindow(Weapon weapon)
    {
        congratsMessage.SetActive(false);

        currentWeapon = weapon.GetWeaponConfig();

        CheckIfUnlocked(weapon.GetWeaponConfig());

        if (!currentWeapon.isUnlocked)
        {
            ShowHidedVersion(currentWeapon.weaponName);
            return;
        }

        killsToUnlockText.gameObject.SetActive(false);
        weaponNameText.text = currentWeapon.catalogueName;
        fireModeText.text = "Fire Mode: " + currentWeapon.FireMode.ToString();
        fireRateText.text = "Fire Rate: " + currentWeapon.fireRate.ToString();
        damageText.text = "Damage: " + currentWeapon.damage.ToString();
        bulletSpeedText.text = "Bullet Speed: " + currentWeapon.bulletSpeed.ToString();
        weightText.text = "Weight: " + currentWeapon.weight.ToString();
        recoilText.text = "Recoil: " + currentWeapon.recoil.ToString();
        clipSizeText.text = "Clip Size: " + currentWeapon.maxAmmo.ToString();
        reloadTimeText.text = "Reload Time: " + currentWeapon.reloadTime.ToString() + " seconds";

        if (weaponHolder.childCount > 0)
            Destroy(weaponHolder.GetChild(0).gameObject);

        Instantiate(currentWeapon.weaponUI, weaponHolder);

        if (pickedWeapons.weapons.Contains(weapon))
            pickButton.interactable = false;
        else
            pickButton.interactable = true;
    }

    private void ShowHidedVersion(string name)
    {
        char[] hiddenName = new char[name.Length];
        for (int i = 0; i < name.Length; i++)
        {
            if (i == 0)
                hiddenName[i] = name[i];
            else
                hiddenName[i] = '?';
        }

        killsToUnlockText.gameObject.SetActive(true);

        if (currentWeapon.isSpecialGun)
        {
            if (currentWeapon.weaponName == "AWP")
            {
                killsToUnlockText.text = "Kill Mustafa!";
            }
            else if (currentWeapon.weaponName == "Rave")
            {
                killsToUnlockText.text = "Kill Alex!";
            }
        }
        else
        {
            killsToUnlockText.text = "Kills to unlock: " +
                                     (currentWeapon.killsToUnlock - PlayerStatsManager.Instance.GetKills());
        }

        weaponNameText.text = new string(hiddenName);
        fireModeText.text = "Fire Mode: " + "???";
        fireRateText.text = "Fire Rate: " + "???";
        damageText.text = "Damage: " + "???";
        bulletSpeedText.text = "Bullet Speed: " + "???";
        weightText.text = "Weight: " + "???";
        recoilText.text = "Recoil: " + "???";
        clipSizeText.text = "Clip Size: " + "???";
        reloadTimeText.text = "Reload Time: " + "???";

        if (weaponHolder.childCount > 0)
            Destroy(weaponHolder.GetChild(0).gameObject);

        pickButton.interactable = false;
    }

    private void UpdatePickedWeapons()
    {
        if (pickedWeapons.weapons.Count > 0)
        {
            pickedWeapon1.text = "First Weapon: " + pickedWeapons.weapons[0].GetWeaponConfig().catalogueName;
        }
        else
        {
            pickedWeapon1.text = "First Weapon: " + "None";
        }

        if (pickedWeapons.weapons.Count > 1)
        {
            pickedWeapon2.text = "Second Weapon: " + pickedWeapons.weapons[1].GetWeaponConfig().catalogueName;
        }
        else
        {
            pickedWeapon2.text = "Second Weapon: " + "None";
        }
    }

    private void CheckIfUnlocked(WeaponConfig weapon)
    {
        if (weapon.isUnlocked) return;

        if (weapon.isSpecialGun)
        {
            if (weapon.weaponName == "AWP" && PlayerStatsManager.Instance.KilledMustafa)
                weapon.SpecialUnlock();
            else if (weapon.weaponName == "Rave" && PlayerStatsManager.Instance.KilledAlex)
                weapon.SpecialUnlock();
        }

        if (PlayerStatsManager.Instance.GetKills() >= weapon.killsToUnlock)
        {
            congratsMessage.SetActive(true);
            weapon.Unlock();
        }
    }
}