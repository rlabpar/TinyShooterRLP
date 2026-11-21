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

    // Se ejecuta al inicializar el componente, antes de Start
    void Awake()
    {
        // Obtiene y guarda el componente NavMeshAgent adjunto a este GameObject
       _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent != null)
        {
            // Asigna la distancia de parada del agente al valor de _stopDistance
            _navMeshAgent.stoppingDistance = _stopDistance;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Busca en la escena una instancia de PlayerInputController para obtener la referencia al jugador
        _player = FindAnyObjectByType<PlayerInputController>();
        if (_player != null && _navMeshAgent != null)
        {
            // Establece la posición inicial del jugador como destino de navegación del enemigo
            _navMeshAgent.SetDestination(_player.transform.position);
        }
        // Inicia la corrutina FireRoutine para que el enemigo dispare periódicamente
        StartCoroutine(FireRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null || _navMeshAgent == null) return;

        // Calcula la distancia en el mundo entre la posición del enemigo y la del jugador
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        // Comprueba si el jugador está más lejos que la distancia de parada configurada
        if (distanceToPlayer > _stopDistance)
        {
            // Permite que el NavMeshAgent reanude o continúe el movimiento
            _navMeshAgent.isStopped = false;
            // Actualiza el destino del agente hacia la posición actual del jugador
            _navMeshAgent.SetDestination(_player.transform.position);
        }
        // Si el jugador está dentro o igual a _stopDistance
        else
        {
            // Detiene el desplazamiento del NavMeshAgent
            _navMeshAgent.isStopped = true;
        }

        // Rotamos al enemigo en el plano horizontal para que mire al jugador sin inclinarse hacia arriba/abajo
        Vector3 lookTarget = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.LookAt(lookTarget); // Orienta el frente (eje Z) del enemigo hacia lookTarget
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.RegisterEnemyDefeated(); // Registra la derrota del enemigo en PortalSpawner al ser tocado por el jugador
            }
            Destroy(this.gameObject); // Cuando colisione el jugador, destruimos al enemigo
        }
    }

    // Gestiona el disparo continuo de cohetes
    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate); // Pausar esta corutina durante unos segundo

            if (_player == null || _rocketSpawnPoint == null || _rocketPrefab == null)
                continue;

            // Instancia el cohete en _rocketSpawnPoint con la rotación del enemigo y obtiene su componente RocketMover
            RocketMover newProjectible = Instantiate(_rocketPrefab, _rocketSpawnPoint.position, this.transform.rotation).GetComponent<RocketMover>();

            if (newProjectible != null)
            {
                // Asigna a este enemigo como dueño del cohete para evitar fuego amigo
                newProjectible.SetOwner(transform);
                // Orienta el cohete hacia la posición horizontal (X, Z) del jugador a la altura Y del enemigo
                newProjectible.transform.LookAt(new Vector3(_player.transform.position.x,
                                                        this.transform.position.y,
                                                        _player.transform.position.z));
            }
        }
    }

    public void SetFireRate(float newFireRate)
    {
        // Comprueba que el nuevo intervalo de disparo sea mayor que 0 para evitar valores inválidos
        if (newFireRate > 0f)
        {
            // Actualiza _fireRate con el nuevo valor recibido
            _fireRate = newFireRate;
        }
    }
}