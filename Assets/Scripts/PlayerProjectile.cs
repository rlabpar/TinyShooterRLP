using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) // O OnCollisionEnter si usas colisión física sin Is Trigger
    {
        // Busca un componente EnemyHealth en el objeto golpeado (other) o en cualquiera de sus objetos padres en la jerarquía, y lo guarda en enemyHealth
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // Llama al método TakeDamage del enemigo pasándole la cantidad de damage
            Destroy(gameObject); // Destruye el GameObject de este proyectil justo después de aplicar el daño
        }
    }
}
