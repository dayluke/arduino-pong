using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody rb;
    public float force;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Assigns random velocity to ball to start.
        float x = Random.Range(-1, 1);
        float y = Random.Range(-1, 1);
        float z = Random.Range(-1, 1);
        rb.velocity = new Vector3(x, y, z) * force;
    }
}
