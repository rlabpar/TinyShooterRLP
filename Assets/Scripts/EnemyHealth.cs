using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ParticleSystem enemyExplosionVFX; //[span_31](start_span)[span_31](end_span)[span_32](start_span)[span_32](end_span)

    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage = 1)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (PortalSpawner.Instance != null)
        {
            PortalSpawner.Instance.RegisterEnemyDefeated();
        }else
        {
            Debug.LogWarning("No hay ningún PortalSpawner en la escena.");
        }

        if (enemyExplosionVFX != null)
        {
            Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
