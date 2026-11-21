using UnityEngine;

public class FinalPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (PortalSpawner.Instance != null)
            {
                PortalSpawner.Instance.OnPortalTouched();
            }
        }
    }
}
