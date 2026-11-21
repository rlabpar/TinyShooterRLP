using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ParticleSystem deathVFX;
    [SerializeField] private float fallThresholdY = -10f;
    
    private int currentHealth;
    private bool isDead;

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

    void Update()
    {
        if (!isDead && transform.position.y < fallThresholdY)
        {
            Die();
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
        if (isDead) return;
        isDead = true;

        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity); //[span_27](start_span)[span_27](end_span)[span_28](start_span)[span_28](end_span)
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

