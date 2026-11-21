using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalSpawner : MonoBehaviour
{
    public static PortalSpawner Instance { get; private set; }

    private const string Level1ScoreKey = "ScoreNivel1";

    // Condiciones de objetivos
    [SerializeField] private int _enemiesToDefeat = 3;
    [SerializeField] private int _coinsToCollect = 30;

    // Flujo por nivel
    [SerializeField] private bool _usePlatformBeforePortal = false; // false en Nivel 1, true en Nivel 2
    [SerializeField] private GameObject _platformObject;
    [SerializeField] private GameObject _portalObject;

    // Cambio de nivel o Pantalla final
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

        if (!_showVictoryCanvasOnTouch) // Si no estamos en el modo de pantalla final
        {
            PlayerPrefs.SetInt(Level1ScoreKey, 0); // reinicia "ScoreNivel1" a 0 en PlayerPrefs
            PlayerPrefs.Save(); // Guarda los cambios
        }

        // Desactivamos al inicio la plataforma final, el portal y el Canvas de victoria
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
        if (_objectivesCompleted) return; // Si los objetivos ya se completaron, no hacemos nada

        _defeatedEnemies++;
        Debug.Log($"Enemigos eliminados: {_defeatedEnemies}/{_enemiesToDefeat} | Coins nivel: {_collectedCoins}/{_coinsToCollect}");
        CheckObjectives();
    }

    public void RegisterCoinCollected(int amount = 1)
    {
        _levelScore += amount; // Suma 1 a la puntuacion del nivel actual

        if (!_showVictoryCanvasOnTouch) // Si estamos en Nivel 1
        {
            // Actualiza y guarda en PlayerPrefs el puntaje del Nivel 1 cada vez que se recoge una moneda
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
        // Si aún no se completaron los objetivos y se alcanzaron o superaron tanto _enemiesToDefeat como _coinsToCollect
        if (!_objectivesCompleted && _defeatedEnemies >= _enemiesToDefeat && _collectedCoins >= _coinsToCollect)
        {
            _objectivesCompleted = true;

            if (_usePlatformBeforePortal) // Si es true
            {
                ActivatePlatform(); // Activamos plataforma
            }
            else // Si es false
            {
                ActivatePortal(); // Activamos portal
            }
        }
    }

    private void ActivatePlatform()
    {
        if (_platformActivated) return; // Evitamos repetir la accion si es true
        _platformActivated = true; // Marcamos el flag

        if (_platformObject != null)
        {
            _platformObject.SetActive(true); // Habilita plataforma en la escena
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

    public void OnPortalTouched() // Para cuando jugador toque portal
    {
        if (_levelFinished) return; // Si el nivel ya terminó, sale
        _levelFinished = true; // Si no, lo marca como terminado para evitar ejecuciones dobles

        if (_showVictoryCanvasOnTouch) // Si _showVictoryCanvasOnTouch es true
        {
            int scoreN1 = PlayerPrefs.GetInt(Level1ScoreKey, 0); // Recupera el puntaje guardado del Nivel 1
            int totalCoins = scoreN1 + _levelScore; // Suma el puntaje de este nivel

            if (_totalScoreText != null)
            {
                _totalScoreText.text = totalCoins.ToString(); // Se actualiza con la suma total convertida a texto
            }

            if (_victoryCanvas != null)
            {
                _victoryCanvas.SetActive(true); // Activamos canva
            }

            // Pausa todo el juego (enemigos, jugador, proyectiles, animaciones basadas en Time.deltaTime, etc.)
            Time.timeScale = 0f;
        }
        else
        {
            // Guarda la puntuación fial del nivel en PlayerPrefs y llama a LoadNextLevel()
            PlayerPrefs.SetInt(Level1ScoreKey, _levelScore);
            PlayerPrefs.Save();
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // Asegura que el tiempo vuelva a correr
        if (!string.IsNullOrEmpty(_nextLevelSceneName))
        {
            SceneManager.LoadScene(_nextLevelSceneName); // Carga siguiente escena
        }
    }
}
