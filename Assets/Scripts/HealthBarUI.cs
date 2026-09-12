using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    // Keeps the TMP display formatted as current/max pairs.
    private const float HealthDisplayDivisor = 20f;

    public float Health, MaxHealth, Width, Height;

    [SerializeField]
    private RectTransform healthBar;

    [SerializeField]
    private TMP_Text healthText;

    public void SetMaxHealth(float maxHealth){
        MaxHealth = maxHealth;
        UpdateHealthText();
    }

    public void SetHealth(float health) {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = $"{Health / HealthDisplayDivisor:0}/{MaxHealth / HealthDisplayDivisor:0}";
    }
}
