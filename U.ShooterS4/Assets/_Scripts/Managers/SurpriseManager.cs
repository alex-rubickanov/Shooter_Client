using System.Collections;
using UnityEngine;

public class SurpriseManager : MonoBehaviour
{
    public static SurpriseManager Instance;
    
    [SerializeField] private GameObject blueScreen;
    [SerializeField] AudioListener audioListener;
    [SerializeField] private float effectTime = 2.0f;
    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowBlueScreen()
    {
        audioListener.enabled = false;
        StartCoroutine(ShowEffect(blueScreen));
    }
    
    
    private IEnumerator ShowEffect(GameObject effect)
    {
        yield return new WaitForSecondsRealtime(effectTime);
        effect.SetActive(true);
        yield return new WaitForSeconds(effectTime);
        effect.SetActive(false);
        audioListener.enabled = true;
    }
}
