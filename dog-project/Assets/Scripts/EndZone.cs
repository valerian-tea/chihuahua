using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [Header("Win UI")]
    public GameObject winText;

    bool hasWon = false;

    void Start()
    {
        if (winText != null)
            winText.SetActive(false);
    }

    void OnTriggerStay(Collider other)
    {
        if (hasWon)
        {
            return;
        }

        if (other.CompareTag("Ball"))
        {
            hasWon = true;

            if (winText != null)
                winText.SetActive(true);
        }
    }
}
