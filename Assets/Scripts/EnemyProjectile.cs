using UnityEngine;

public class EnemyProjectile : MonoBehaviour
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
        // Busca el componente PlayerHealth en el objeto colisionado o en cualquiera de sus objetos padres
        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        
        if (playerHealth != null)
        {
            // Aplica la cantidad de daño configurada al jugador llamando a su método TakeDamage
            playerHealth.TakeDamage(damage);
            // Destruye el GameObject de este proyectil tras impactar al jugador
            Destroy(gameObject);
        }
    }
}
