using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        Debug.Log($"HealthBar SetMaxHealth: {health}");
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        Debug.Log($"HealthBar SetHealth: {health}");
    }
}
