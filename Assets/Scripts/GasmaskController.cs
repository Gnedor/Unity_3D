using UnityEngine;

public class GasmaskController : MonoBehaviour
{
    public float speed = 1.5f;
    public float detectionRadius = 5f;
    public float shootingRadius = 2f;
    public float fireRate = 1.5f; // Time in seconds between shots
    public GameObject bulletPrefab;
    public Transform firePoint;

    private GameObject player;
    private Animator animator;
    private float nextFireTime = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= shootingRadius)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isInRange", true);

            FacePlayer();

            if (Time.time >= nextFireTime)
            {
                ShootAtPlayer();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            animator.SetBool("isWalking", true);
            animator.SetBool("isInRange", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isInRange", false);
        }
    }

    void FacePlayer()
    {
        Vector3 scale = transform.localScale;
        if (player.transform.position.x < transform.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 direction = (player.transform.position - firePoint.position).normalized;
            direction.z = 0; // Use this if you're working in the XY plane
            // direction.y = 0; // Use this if you're in top-down (XZ) setup

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = direction * 10f;
            }

            // Optional: flip bullet sprite visually if needed
            if (direction.x < 0)
            {
                Vector3 scale = bullet.transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                bullet.transform.localScale = scale;
            }
        }
    }
}