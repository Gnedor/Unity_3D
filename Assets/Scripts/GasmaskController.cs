using UnityEngine;

public class GasmaskController : MonoBehaviour
{
    public float speed = 1.5f;
    public float detectionRadius = 5f;
    public float shootingRadius = 2f;

    private GameObject player;
    private Animator animator;

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
            // Stop moving and enter aiming/shooting state
            animator.SetBool("isWalking", false);
            animator.SetBool("isInRange", true);
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            // Move toward player
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            animator.SetBool("isWalking", true);
            animator.SetBool("isInRange", false);
        }
        else
        {
            // Player is out of detection range
            animator.SetBool("isWalking", false);
            animator.SetBool("isInRange", false);
        }
    }
}