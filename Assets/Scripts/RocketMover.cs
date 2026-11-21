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
        _ownerRoot = owner != null ? owner.root : null;
        _firedByPlayer = _ownerRoot != null &&
                        (_ownerRoot.CompareTag("Player") || owner.CompareTag("Player"));
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidbody.linearVelocity = transform.forward * _speed;
        Destroy(this.gameObject, _lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el objeto que toca pertenece a quien disparó este cohete, no lo destruyas
        if (_ownerRoot != null && other.transform.root == _ownerRoot)
            return;

        bool esPlayer = other.CompareTag("Player") || other.transform.root.CompareTag("Player");
        bool esEnemy = other.CompareTag("Enemy") || other.transform.root.CompareTag("Enemy");
        bool esGround = other.gameObject.layer == LayerMask.NameToLayer("Ground");
        bool esWall = other.gameObject.layer == LayerMask.NameToLayer("Wall");

        // Solo un cohete disparado por un Enemy (o no disparado por Player) daña al Player
        if (esPlayer && !_firedByPlayer)
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }

        // Solo un cohete disparado por el Player daña a los Enemy
        if (esEnemy && _firedByPlayer)
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }

        if (esPlayer || esEnemy || esGround || esWall)
        {
            Destroy(this.gameObject);
        }
    }
}
