using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private Slider healthBar;
    [SerializeField] private ParticleSystem enemyExplosionVFX;

    private int currentHealth;

    void Start()
    {
        // Inicializa la vida actual con el valor de vida máxima
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            // Establece el valor mínimo de la barra de vida en 0
            healthBar.minValue = 0;
            // Establece el valor máximo de la barra de vida igual a maxHealth
            healthBar.maxValue = maxHealth;
            // Sincroniza el valor actual visible de la barra con currentHealth
            healthBar.value = currentHealth;
        }
    }

    // Método público para aplicar daño al enemigo (por defecto resta 1 si no se indica cantidad)
    public void TakeDamage(int damage = 1)
    {
        // Resta el daño recibido a la vida actual
        currentHealth -= damage;
        // Evita que la vida actual baje de 0 usando Mathf.Max
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
        {
            healthBar.value = currentHealth; // Actualiza su valor con la vida actual
        }

        if (currentHealth <= 0)
        {
            Die(); // Llama al método Die para gestionar la muerte del enemigo
        }
    }

    private void Die()
    {
        if (PortalSpawner.Instance != null)
        {
            PortalSpawner.Instance.RegisterEnemyDefeated(); // Notifica a PortalSpawner que se ha derrotado a un enemigo
        }else
        {
            Debug.LogWarning("No hay ningún PortalSpawner en la escena.");
        }

        if (enemyExplosionVFX != null)
        {
            // Instancia el sistema de partículas en la posición del enemigo sin rotación adicional
            Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);
        }

        // Destruye el GameObject de este enemigo para eliminarlo de la escena
        Destroy(this.gameObject);
    }
}
