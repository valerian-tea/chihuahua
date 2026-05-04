using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public GameObject projectile;
    public float launchVelocity = 100f;
    public float speed = 10f;
    public float frequency = 5f;
    public float amplitude = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Launching projectile");
            Instantiate(projectile, transform.position, transform.rotation);
        }
    }
}
