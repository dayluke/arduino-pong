using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assigns random velocity to ball to start.
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        rb.velocity = new Vector3(x, y, z);
    }
}
