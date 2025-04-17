using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public BossController boss;
    public Slider healthSlider;

    void Start()
    {
        if (boss != null && healthSlider != null)
        {
            healthSlider.maxValue = boss.maxHealth;
            healthSlider.value = boss.currentHealth;
        }
    }

    void Update()
    {
        if (boss != null && healthSlider != null)
        {
            healthSlider.value = boss.currentHealth;
        }
    }
}
