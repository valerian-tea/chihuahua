using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f;
    public float frequency = 5f;
    public float amplitude = 2f;

    void Update()
    {
        // Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Oscillate sideways
        float zigZag = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.Translate(Vector3.right * zigZag * Time.deltaTime);
    }
}
