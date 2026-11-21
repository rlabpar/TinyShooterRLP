using UnityEngine;

public class GoldPicker : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si ni el objeto tocado ni su raíz tienen la etiqueta "Player", sale del método sin recoger la moneda
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player"))
            return;

        // Busca en la escena un componente de tipo ScoreBoardManager
        ScoreBoardManager scoreBoardManager = FindAnyObjectByType<ScoreBoardManager>();
        if (scoreBoardManager != null)
        {
            scoreBoardManager.IncreaseScore(coinValue); // Incrementa la puntuación en el marcador según el valor de la moneda
        }

        if (PortalSpawner.Instance != null)
        {
            PortalSpawner.Instance.RegisterCoinCollected(coinValue); // Registra las monedas recogidas en PortalSpawner para comprobar objetivos y puntuación del nivel
        }

        Destroy(this.gameObject); // Destruye el GameObject de esta moneda para que desaparezca de la escena tras recogerla
    }
}