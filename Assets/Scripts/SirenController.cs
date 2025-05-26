using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
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
    public float summonTimer = 20.0f;
    private bool spawning = false;
    public TextMeshProUGUI bossText;
    public GameObject bossHealthBar;
    private GameObject bar;
    private RectTransform hBar;
    private int maxHealth;

    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        maxHealth = enemyScript.health;

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
        bar = bossHealthBar.transform.GetChild(0).gameObject;
        hBar = bar.GetComponent<RectTransform>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if ((distanceToPlayer <= detectionRadius || enemyScript.follow) && !spawning)
        {
            bossText.text = "Siren";
            bossHealthBar.SetActive(true);
            enemyScript.follow = true;

            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);

            if (!isPlayingSound)
            {
                audioSource.Play();
                isPlayingSound = true;
            }
        }
        else
        {
            if (isPlayingSound)
            {
                audioSource.Stop();
                isPlayingSound = false;
            }
        }

        if (enemyScript.follow)
        {
            if (summonTimer > 0)
            {
                summonTimer -= Time.deltaTime;
            }
            else if (!spawning)
            {
                StartCoroutine(SummonEnemies());
            }
        }
        float width = (float)enemyScript.health / (float)maxHealth * 300f;
        hBar.sizeDelta = new Vector2(width, 35f);

    }

    IEnumerator SummonEnemies()
    {
        spawning = true;
        animator.SetBool("isAttacking", true);

        if (!isPlayingSound)
        {
            audioSource.Play();
            isPlayingSound = true;
        }
        yield return new WaitForSeconds(3);

        for (int i = 0; i <= 6; i++)
        {
            int random = UnityEngine.Random.Range(0, 4);
            float randomPosX = UnityEngine.Random.Range(-10, 10);
            float randomPosZ = UnityEngine.Random.Range(-10, 10);
            Instantiate(enemies[random], new Vector3(transform.position.x + randomPosX, 10, transform.position.z + randomPosZ), transform.rotation);
        }

        animator.SetBool("isAttacking", false);
        audioSource.Stop();
        isPlayingSound = false;
        summonTimer = 20.0f;
        spawning = false;
    }
}