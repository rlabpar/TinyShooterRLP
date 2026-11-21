using UnityEngine;

public class PlatformRevealPortal : MonoBehaviour
{
    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        TriggerPortal(other); // // Llama al método común pasando el Collider del otro objeto
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerPortal(collision.collider); // // Llama al método común pasando el Collider involucrado en la colisión
    }

    private void TriggerPortal(Collider other)
    {
        if (_triggered || other == null) return;

        // Comprueba si el objeto tocado o su objeto raíz tienen la etiqueta "Player"
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            _triggered = true;

            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.ActivatePortal(); // Activa el portal llamando a PortalSpawner
            }
        }
    }
}