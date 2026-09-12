using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Current and maximum values for both the player and the enemy.
    public float Health, MaxHealth, Enemy_Health, Enemy_MaxHealth;

    [SerializeField]
    private HealthBarUI healthBar;
    [SerializeField]
    private HealthBarUI healthBar_Enemy;

    // Initializes both health bars and the starting health values when the scene starts.
    void Start()
    {
        Health = MaxHealth;
        Enemy_Health = Enemy_MaxHealth;

        healthBar.SetMaxHealth(MaxHealth);
        healthBar_Enemy.SetMaxHealth(Enemy_MaxHealth);
        healthBar.SetHealth(Health);
        healthBar_Enemy.SetHealth(Enemy_Health);
    }

    // Allows quick manual health testing during development.
    void Update()
    {
        if(Input.GetKeyDown("d")) {
            SetHealth(-20f);
        }

        if(Input.GetKeyDown("h")) {
            SetHealth(20f);
        }

        if(Input.GetKeyDown("f")) {
            SetEnemyHealth(-20f);
        }

        if(Input.GetKeyDown("g")) {
            SetEnemyHealth(20f);
        }

    }

    // Applies a health change to the player and clamps the value to the valid range.
    public void SetHealth(float healthChange) {
        Health += healthChange;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        healthBar.SetHealth(Health);
    }

    // Applies a health change to the enemy and clamps the value to the valid range.
    public void SetEnemyHealth(float healthChange) {
        Enemy_Health += healthChange;
        Enemy_Health = Mathf.Clamp(Enemy_Health, 0, Enemy_MaxHealth);

        healthBar_Enemy.SetHealth(Enemy_Health);
    }
}
