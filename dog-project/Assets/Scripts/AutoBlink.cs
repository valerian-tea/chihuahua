using System.Collections;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int blink = 0;
    public float blinkSpeed = 10.0f;
    public float minWaitTime = 1.0f;
    public float maxWaitTime = 8.0f;

    void Start()
    {
        // Ensure you have the SkinnedMeshRenderer reference
        if (skinnedMeshRenderer == null)
        {
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Wait for a random interval before the next blink
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

            // Close eyes
            float weight = 0f;
            while (weight < 100f)
            {
                weight += blinkSpeed * Time.deltaTime * 100f / blinkSpeed; // Smooth transition
                weight = Mathf.Clamp(weight, 0f, 100f);
                skinnedMeshRenderer.SetBlendShapeWeight(blink, weight);
                yield return null;
            }

            // Keep eyes closed briefly
            yield return new WaitForSeconds(0.1f);

            // Open eyes
            while (weight > 0f)
            {
                weight -= blinkSpeed * Time.deltaTime * 100f / blinkSpeed; // Smooth transition back
                weight = Mathf.Clamp(weight, 0f, 100f);
                skinnedMeshRenderer.SetBlendShapeWeight(blink, weight);
                yield return null;
            }
        }
    }
}
