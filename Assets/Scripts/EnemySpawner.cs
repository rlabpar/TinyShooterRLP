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
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (_spawnedCount < _maxEnemiesToSpawn)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (_enemyPrefab)
            {
                // Instancia el enemigo en la posición y rotación del spawner
                Instantiate(_enemyPrefab, transform.position, transform.rotation);
                _spawnedCount++;            
            }
        }
    }
}
