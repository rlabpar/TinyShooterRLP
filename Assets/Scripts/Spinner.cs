using UnityEngine;

public class Spinner : MonoBehaviour
{

        private float xAngle = 0f;
        private float yAngle = 90f;
        private float zAngle = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(xAngle * Time.deltaTime, yAngle * Time.deltaTime, zAngle * Time.deltaTime);
        }
}
