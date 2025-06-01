using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkelettController : MonoBehaviour
{
    public AudioClip knifeSound;
    private AudioSource audioSource;
    private GameObject player;
    private EnemyScript enemyScript;
    private float detectionRadius = 20f;
    private float attackRadius = 3f;
    private Animator anim;
    private PlayerController playerController;
    private bool canAttack = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = knifeSound;
        audioSource.playOnAwake = false;

        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if ((!enemyScript.follow && distanceToPlayer <= detectionRadius))
        {
            enemyScript.follow = true;
        }

        if (distanceToPlayer <= attackRadius)
        {
            if (canAttack)
            {
                canAttack = false;
                audioSource.Play();
                StartCoroutine(Attack());
            }

        }
        if (enemyScript.follow)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);
        }

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= 5f)
        {
            playerController.TakeDamage();
        }
        canAttack = true;
    }
}
