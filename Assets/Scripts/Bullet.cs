using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Make sure we have a rigidbody
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configure the rigidbody for a bullet
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Set a lifetime for the bullet
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 velocity)
    {
        if (rb != null)
        {
            // Use velocity instead of linearVelocity
            rb.linearVelocity = velocity;

            // Optional: Make the bullet face the direction it's moving
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity.normalized;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            // Here you would typically apply damage to the player
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.isTrigger) // Avoid destroying on enemy or other triggers
        {
            Destroy(gameObject);
        }
    }

    // Additional collision handling for physical collisions
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}