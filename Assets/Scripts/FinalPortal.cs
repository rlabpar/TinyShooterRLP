using UnityEngine;

public class FinalPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que tocó el portal o su objeto raíz tienen la etiqueta "Player"
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (PortalSpawner.Instance != null)
            {
                // Llama a OnPortalTouched para gestionar la finalización del nivel (canvas final o cambio de escena)
                PortalSpawner.Instance.OnPortalTouched();
            }
        }
    }
}
