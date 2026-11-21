using UnityEngine;

public class PlatformRevealPortal : MonoBehaviour
{
    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        TriggerPortal(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TriggerPortal(collision.collider);
    }

    private void TriggerPortal(Collider other)
    {
        if (_triggered || other == null) return;

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            _triggered = true;

            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.ActivatePortal();
            }
        }
    }
}