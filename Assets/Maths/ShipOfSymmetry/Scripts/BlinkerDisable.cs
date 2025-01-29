using UnityEngine;

public class BlinkerDisable : MonoBehaviour
{
    private bool isInvokeScheduled = false;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
        if (gameObject.activeSelf && !isInvokeScheduled)
        {
            Invoke("DisableBlinker", 10f); 
            isInvokeScheduled = true;
        }
    }

    private void DisableBlinker()
    {
        gameObject.SetActive(false);
    }
}
