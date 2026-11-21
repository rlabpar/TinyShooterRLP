using UnityEngine;
using TMPro;

public class ScoreBoardManager : MonoBehaviour
{

    private int score;
    public TMP_Text scoreBoardText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0; // Inicializamos la puntuación a 0 al empezar la partida
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        scoreBoardText.text = score.ToString(); // Actualiza el texto en pantalla
    }
}
