using UnityEngine;

public class RocketMover : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float _speed = 20f;
    [SerializeField] private float _lifeTime = 2f;
    private Transform _ownerRoot;
    private bool _firedByPlayer;

    public void SetOwner(Transform owner)
    {
        // Si owner no es nulo, guarda owner.root (el objeto raíz de esa jerarquía); si es nulo, asigna null
        _ownerRoot = owner != null ? owner.root : null;
        _firedByPlayer = _ownerRoot != null &&
                        (_ownerRoot.CompareTag("Player") || owner.CompareTag("Player")); // Comprueba si _ownerRoot tiene la etiqueta "Player". El resultado (true o false) se guarda en _firedByPlayer
    }

    void Awake() // Se ejecuta al inicializar el objeto (antes de Start)
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidbody.linearVelocity = transform.forward * _speed; // Impulsa el cohete hacia adelante a la velocidad speed
        Destroy(this.gameObject, _lifeTime); // Destruye proyectiles para que no se queden en la escena
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el objeto que toca pertenece a quien disparó este cohete, no lo destruyas
        if (_ownerRoot != null && other.transform.root == _ownerRoot)
            return;

        bool esPlayer = other.CompareTag("Player") || other.transform.root.CompareTag("Player"); // Si tiene tag "Player"
        bool esEnemy = other.CompareTag("Enemy") || other.transform.root.CompareTag("Enemy"); // Si tiene tag "Enemy"
        bool esGround = other.gameObject.layer == LayerMask.NameToLayer("Ground"); // Si esta en capa "Ground"
        bool esWall = other.gameObject.layer == LayerMask.NameToLayer("Wall"); // Si esta en capa "Wall"

        if (esPlayer && !_firedByPlayer) // Si se tocó al jugador y el cohete no fue disparado por el jugador
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // resta 1 de vida llamando a TakeDamage(1)
            }
        }

        // Solo un cohete disparado por el Player daña a los Enemy
        if (esEnemy && _firedByPlayer) // Si se tocó a un enemigo y el cohete sí fue disparado por el jugador
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // +1 punto de dañp
            }
        }

        if (esPlayer || esEnemy || esGround || esWall) // Si el impacto fue contra un jugador, un enemigo, el suelo o una pared
        {
            Destroy(this.gameObject); // Destruye el GameObject del cohete
        }
    }
}
