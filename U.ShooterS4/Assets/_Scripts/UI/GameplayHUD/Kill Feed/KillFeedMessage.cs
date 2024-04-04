using TMPro;
using UnityEngine;

public class KillFeedMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killerNameText;
    [SerializeField] private TextMeshProUGUI killedNameText;
    
    
    public void SetNames(string killer, string killed)
    {
        killerNameText.text = killer;
        killedNameText.text = killed;
    }
}
