using System.Collections;
using UnityEngine;

public class ScientistController : MonoBehaviour
{
    public AudioClip knifeSound;
    private AudioSource audioSource;
    public float detectionRadius;
    private GameObject player;
    private Animator animator;
    private EnemyScript enemyScript;
    private float attackRadius = 3f;
    private PlayerController playerController;
    private bool canAttack = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        audioSource.clip = knifeSound;
        audioSource.playOnAwake = false;
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

        if (distanceToPlayer <= attackRadius)
        {
            if (canAttack)
            {
                canAttack = false;
                audioSource.Play();
                animator.SetBool("isAttacking", true);
                StartCoroutine(Attack());
            }

        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= 5f)
        {
            playerController.TakeDamage();
        }
        canAttack = true;
    }
}