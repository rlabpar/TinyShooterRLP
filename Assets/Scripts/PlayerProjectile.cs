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
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
