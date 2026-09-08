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

    // Resize the fill bar and refresh the text for the new health value.
    public void SetHealth(float health) {
        Health = health;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
            healthText.text = $"{Health / HealthDisplayDivisor:0}/{MaxHealth / HealthDisplayDivisor:0}";
    }
}
