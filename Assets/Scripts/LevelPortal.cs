using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que tocó el portal o su objeto raíz tienen la etiqueta "Player"
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            // // Verifica que exista una instancia activa de PortalSpawner en la escena
            if (PortalSpawner.Instance != null)
            {
                // Llama a LoadNextLevel para cargar la siguiente escena
                PortalSpawner.Instance.LoadNextLevel();
            }
        }
    }
}
