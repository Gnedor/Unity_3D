using System.Collections.Generic;
using UnityEngine;
public class SirenController : MonoBehaviour
{
    public float detectionRadius = 5f;

    // Audio components
    public AudioClip sirenSound;
    private AudioSource audioSource;
    private bool isPlayingSound = false;

    private GameObject player;
    private Animator animator;
    private EnemyScript enemyScript;
    public List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if it doesn't exist
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure audio source
        audioSource.clip = sirenSound;
        audioSource.loop = true; // Loop the sound while attacking
        audioSource.playOnAwake = false;
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
            animator.SetBool("isAttacking", true);

            // Play sound if not already playing
            if (!isPlayingSound)
            {
                audioSource.Play();
                isPlayingSound = true;
            }
            SummonEnemies();
        }
        else
        {
            // Player is out of range, stop walking
            animator.SetBool("isAttacking", false);

            // Stop sound if it's playing
            if (isPlayingSound)
            {
                audioSource.Stop();
                isPlayingSound = false;
            }
        }
    }

    //Siren kan vara den sista bossen som spawnar massa andra fiender runt omkring honnom
    void SummonEnemies()
    {
        for (int i = 0; i <= 6; i++)
        {
            int random = UnityEngine.Random.Range(0, 4);
            GameObject newEnemy = Instantiate(enemies[random], transform.position, transform.rotation);
        }
    }
}