using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleDataSerialization : MonoBehaviour
{
    public static GoogleDataSerialization Instance;
    public static string PlayerName = "";
    public static int Kills = 0;
    
    [SerializeField] private AllWeapons pickedWeapons;
    private readonly string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdRsyw7sXVGpH65Qq6RwNgFxMXOnGWgu_gnAc1lrBH3zfXPHQ/formResponse";

    private void Awake()
    {
        Instance = this;
    }

    public void SendData()
    {
        PlayerName = Client.Instance.PlayerData.Name;
        if (pickedWeapons.weapons.Count != 0)
        {
            if (pickedWeapons.weapons.Count == 1)
            {
                StartCoroutine(Post(PlayerName, pickedWeapons.weapons[0].name, ""));
            }
            else
            {
                StartCoroutine(Post(PlayerName, pickedWeapons.weapons[0].name, pickedWeapons.weapons[1].name));
            }
        }
    }

    private IEnumerator Post(string playerName, string firstWeapon, string secondWeapon)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1788589170", playerName);
        form.AddField("entry.1251130489", firstWeapon);
        form.AddField("entry.1705016621", secondWeapon);
        form.AddField("entry.612893492", Kills);
        
        
        UnityWebRequest request = UnityWebRequest.Post(URL, form);
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Form upload complete!");
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}