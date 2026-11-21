using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float fallThresholdY = -10f;
    
    private int currentHealth;
    private bool isDead;

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

    void Update()
    {
        // Si el jugador aún no está muerto y su posición en Y está por debajo del límite de caída
        if (!isDead && transform.position.y < fallThresholdY)
        {
            // Ejecuta la muerte del jugador por caída al vacío
            Die();
        }
    }

    // Aplicar daño al jugador
    public void TakeDamage(int damage = 1)
    {
        // Resta la cantidad de daño recibida a la vida actual
        currentHealth -= damage;
        // Evita que la vida actual baje de 0 usando Mathf.Max
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
        {
            healthBar.value = currentHealth; // Actualiza su valor con la vida actual
        }

        // Comprueba si la vida actual ha llegado a 0 o menos
        if (currentHealth <= 0)
        {
            // Llama al método Die para gestionar la muerte del jugador
            Die();
        }
    }

    private void Die()
    {
        // Si el jugador ya murió previamente, sale del método para evitar repetir la lógica
        if (isDead) return;
        // Marca al jugador como muerto
        isDead = true;

        // Recarga la escena activa actualmente para reiniciar el nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}