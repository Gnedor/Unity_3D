using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float detectionRadius;

    private GameObject player;
    private Animator animator;
    private EnemyScript enemyScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        enemyScript = GetComponent<EnemyScript>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (!enemyScript.follow && distanceToPlayer <= detectionRadius)
        {
            enemyScript.follow = true;
        }
        
        if (enemyScript.follow)
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