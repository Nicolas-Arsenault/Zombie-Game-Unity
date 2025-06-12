using UnityEngine;
using UnityEngine.UI;

public class ZombieHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentHealth, float maxVal)
    {
        slider.value = currentHealth / maxVal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
