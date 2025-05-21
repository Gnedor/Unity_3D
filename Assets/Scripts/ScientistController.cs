using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public float detectionRadius = 5f;
    private GameObject player;
    private Animator animator;
    private EnemyScript enemyScript;

    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Move towards the player
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);

            // Set walking animation to true
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Player is out of range, stop walking
            animator.SetBool("isWalking", false);
        }
    }
}