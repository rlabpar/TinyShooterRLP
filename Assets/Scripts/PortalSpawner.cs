using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSpawner : MonoBehaviour
{
    public static PortalSpawner Instance { get; private set; }

    [SerializeField] private int _enemiesToDefeat = 3;
    [SerializeField] private int _coinsToCollect = 30;

    [SerializeField] private GameObject _portalObject; // Objeto del portal en la escena (empieza desactivado) o prefab
    [SerializeField] private string _nextLevelSceneName = "Nivel2";

    private int _defeatedEnemies = 0;
    private int _collectedCoins = 0;
    private bool _portalActivated = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_portalObject != null)
        {
            _portalObject.SetActive(false);
        }
    }

    public void RegisterEnemyDefeated()
    {
        if (_portalActivated) return;

        _defeatedEnemies++;
        Debug.Log($"Enemigos eliminados: {_defeatedEnemies}/{_enemiesToDefeat}");
        CheckPortalCondition();
    }

    public void RegisterCoinCollected(int amount = 1)
    {
        if (_portalActivated) return;

        _collectedCoins += amount;
        Debug.Log($"Enemigos eliminados: {_defeatedEnemies}/{_enemiesToDefeat} | Coins: {_collectedCoins}/{_coinsToCollect}");
        CheckPortalCondition();
    }

    private void CheckPortalCondition()
    {
        if (!_portalActivated && _defeatedEnemies >= _enemiesToDefeat && _collectedCoins >= _coinsToCollect)
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        _portalActivated = true;

        if (_portalObject != null)
        {
            _portalObject.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(_nextLevelSceneName))
        {
            SceneManager.LoadScene(_nextLevelSceneName);
        }
    }
}