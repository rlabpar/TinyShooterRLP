using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalSpawner : MonoBehaviour
{
    public static PortalSpawner Instance { get; private set; }

    private const string Level1ScoreKey = "ScoreNivel1";

    [Header("Condiciones de objetivos")]
    [SerializeField] private int _enemiesToDefeat = 3;
    [SerializeField] private int _coinsToCollect = 30;

    [Header("Flujo por nivel")]
    [SerializeField] private bool _usePlatformBeforePortal = false; // false en Nivel 1, true en Nivel 2
    [SerializeField] private GameObject _platformObject;
    [SerializeField] private GameObject _portalObject;

    [Header("Cambio de nivel o Pantalla final")]
    [SerializeField] private bool _showVictoryCanvasOnTouch = false; // false en Nivel 1, true en Nivel 2
    [SerializeField] private string _nextLevelSceneName = "Nivel2";
    [SerializeField] private GameObject _victoryCanvas;
    [SerializeField] private TMP_Text _totalScoreText;

    private int _defeatedEnemies = 0;
    private int _collectedCoins = 0;
    private int _levelScore = 0;
    private bool _objectivesCompleted = false;
    private bool _platformActivated = false;
    private bool _portalActivated = false;
    private bool _levelFinished = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Aseguramos que el tiempo corra normal al cargar o reiniciar la escena
        Time.timeScale = 1f;

        if (!_showVictoryCanvasOnTouch)
        {
            PlayerPrefs.SetInt(Level1ScoreKey, 0);
            PlayerPrefs.Save();
        }

        if (_platformObject != null)
        {
            _platformObject.SetActive(false);
        }

        if (_portalObject != null)
        {
            _portalObject.SetActive(false);
        }

        if (_victoryCanvas != null)
        {
            _victoryCanvas.SetActive(false);
        }
    }

    public void RegisterEnemyDefeated()
    {
        if (_objectivesCompleted) return;

        _defeatedEnemies++;
        Debug.Log($"Enemigos eliminados: {_defeatedEnemies}/{_enemiesToDefeat} | Coins nivel: {_collectedCoins}/{_coinsToCollect}");
        CheckObjectives();
    }

    public void RegisterCoinCollected(int amount = 1)
    {
        _levelScore += amount;

        if (!_showVictoryCanvasOnTouch)
        {
            PlayerPrefs.SetInt(Level1ScoreKey, _levelScore);
            PlayerPrefs.Save();
        }

        Debug.Log($"Moneda recogida (+{amount}). Score en este nivel: {_levelScore} | Guardado Nivel 1: {PlayerPrefs.GetInt(Level1ScoreKey, 0)}");

        if (_objectivesCompleted) return;

        _collectedCoins += amount;
        Debug.Log($"Enemigos eliminados: {_defeatedEnemies}/{_enemiesToDefeat} | Coins nivel: {_collectedCoins}/{_coinsToCollect}");
        CheckObjectives();
    }

    private void CheckObjectives()
    {
        if (!_objectivesCompleted && _defeatedEnemies >= _enemiesToDefeat && _collectedCoins >= _coinsToCollect)
        {
            _objectivesCompleted = true;

            if (_usePlatformBeforePortal)
            {
                ActivatePlatform();
            }
            else
            {
                ActivatePortal();
            }
        }
    }

    private void ActivatePlatform()
    {
        if (_platformActivated) return;
        _platformActivated = true;

        if (_platformObject != null)
        {
            _platformObject.SetActive(true);
        }
    }

    public void ActivatePortal()
    {
        if (_portalActivated) return;
        _portalActivated = true;

        if (_portalObject != null)
        {
            _portalObject.SetActive(true);
        }
    }

    public void OnPortalTouched()
    {
        if (_levelFinished) return;
        _levelFinished = true;

        if (_showVictoryCanvasOnTouch)
        {
            int scoreN1 = PlayerPrefs.GetInt(Level1ScoreKey, 0);
            int totalCoins = scoreN1 + _levelScore;

            if (_totalScoreText != null)
            {
                _totalScoreText.text = totalCoins.ToString();
            }

            if (_victoryCanvas != null)
            {
                _victoryCanvas.SetActive(true);
            }

            // Pausa todo el juego (enemigos, jugador, proyectiles, animaciones basadas en Time.deltaTime, etc.)
            Time.timeScale = 0f;
        }
        else
        {
            PlayerPrefs.SetInt(Level1ScoreKey, _levelScore);
            PlayerPrefs.Save();
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(_nextLevelSceneName))
        {
            SceneManager.LoadScene(_nextLevelSceneName);
        }
    }
}
