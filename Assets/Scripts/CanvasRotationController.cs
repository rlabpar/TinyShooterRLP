using UnityEngine;

public class CanvasRotationController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() // Se ejecuta después de Update en cada fotograma
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation; // Iguala la rotación global de este objeto a la rotación global de la cámara principal
                                                                 // Así el Canvas queda paralelo al plano de visión de la cámara
        }
    }
}
