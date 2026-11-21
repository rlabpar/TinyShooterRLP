using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab; // Arrastra aquí el prefab del Enemy
    [SerializeField] private float _spawnInterval = 5f; // Tiempo entre spawns (ej. 5 segundos)

    [SerializeField] private int _maxEnemiesToSpawn = 3;   // Máximo de enemigos que creará este spawner en el nivel

    private int _spawnedCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicia la corrutina SpawnRoutine para empezar a generar enemigos periódicamente
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        // Repite el ciclo mientras no se haya alcanzado el máximo de enemigos a generar
        while (_spawnedCount < _maxEnemiesToSpawn)
        {
            // Pausa la ejecución de la corrutina durante _spawnInterval segundos antes de continuar
            yield return new WaitForSeconds(_spawnInterval);

            if (_enemyPrefab)
            {
                // Instancia el enemigo en la posición y rotación del spawner
                Instantiate(_enemyPrefab, transform.position, transform.rotation);
                // Incrementa en 1 el contador de enemigos generados
                _spawnedCount++;            
            }
        }
    }
}
