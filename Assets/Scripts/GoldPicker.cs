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
        if (!other.CompareTag("Player") && !other.transform.root.CompareTag("Player"))
            return;

        ScoreBoardManager scoreBoardManager = FindAnyObjectByType<ScoreBoardManager>();
        if (scoreBoardManager != null)
        {
            scoreBoardManager.IncreaseScore(coinValue);
        }

        if (PortalSpawner.Instance != null)
        {
            PortalSpawner.Instance.RegisterCoinCollected(coinValue);
        }

        Destroy(this.gameObject);
    }
}