using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Slider healthSlider;
    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }
    
    public void SetMaxHealth(float health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    
    public void UpdateHealth(float health)
    {
        if (health <= 0)
        {
            healthSlider.value = 0;
        }
        healthSlider.value = health;
    }
}
