using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyNavigator : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private PlayerInputController _player;

    [SerializeField] private float _stopDistance = 5f;

    // Disparo
    [SerializeField] private float _fireRate = 5f;
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _rocketSpawnPoint;

    void Awake()
    {
       _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent != null)
        {
            _navMeshAgent.stoppingDistance = _stopDistance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindAnyObjectByType<PlayerInputController>();
        if (_player != null && _navMeshAgent != null)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
        }
        StartCoroutine(FireRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null || _navMeshAgent == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (distanceToPlayer > _stopDistance)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_player.transform.position);
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }

        // Rotamos al enemigo en el plano horizontal para que mire al jugador sin inclinarse hacia arriba/abajo
        Vector3 lookTarget = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.LookAt(lookTarget);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.RegisterEnemyDefeated();
            }
            Destroy(this.gameObject); // Cuando colisione el jugador, destruimos al enemigo
        }
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate); // Pausar esta corutina durante unos segundo

            if (_player == null || _rocketSpawnPoint == null || _rocketPrefab == null)
                continue;

            RocketMover newProjectible = Instantiate(_rocketPrefab, _rocketSpawnPoint.position, this.transform.rotation).GetComponent<RocketMover>();

            if (newProjectible != null)
            {
                newProjectible.SetOwner(transform);
                newProjectible.transform.LookAt(new Vector3(_player.transform.position.x,
                                                        this.transform.position.y,
                                                        _player.transform.position.z));
            }
        }
    }

    public void SetFireRate(float newFireRate)
    {
        if (newFireRate > 0f)
        {
            _fireRate = newFireRate;
        }
    }

}