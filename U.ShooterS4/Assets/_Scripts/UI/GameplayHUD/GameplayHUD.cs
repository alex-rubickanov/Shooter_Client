using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    public static GameplayHUD Instance;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private WeaponUI weaponUI;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private PauseUI pauseUI;

    private bool isPause = false;
    
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
        
        inputReader.OnPauseEvent += TogglePause;
        pauseUI.Close();
    }

    private void TogglePause()
    {
        if (isPause)
        {
            pauseUI.Close();
        }
        else
        {
            pauseUI.Open();
        }

        isPause = !isPause;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        pauseUI.Close();
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
    
    public void UpdateHealth(float health)
    {
        healthUI.UpdateHealth(health);
    }
    
    public void SetMaxHealth(float health)
    {
        healthUI.SetMaxHealth(health);
    }
}