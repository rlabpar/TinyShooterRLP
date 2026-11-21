using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // En tus pruebas anteriores, el collider del jugador era PlayerBody o su raíz tenía el tag Player[span_0](start_span)[span_0](end_span)
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.LoadNextLevel();
            }
        }
    }
}
