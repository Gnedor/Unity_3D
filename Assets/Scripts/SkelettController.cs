using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkelettController : MonoBehaviour
{
    private GameObject player;
    private EnemyScript enemyScript;
    private float detectionRadius = 20f;
    private float attackRadius = 3f;
    private Animator anim;
    private PlayerController playerController;
    private bool follow;
    private int startHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        startHealth = enemyScript.health;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if ((!follow && distanceToPlayer <= detectionRadius) || enemyScript.health < startHealth)
        {
            follow = true;
        }

        if (distanceToPlayer <= attackRadius)
        {
            StartCoroutine(Attack());
        }
        if (follow)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);
        }

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= 5f)
        {
            playerController.TakeDamage();
        }
    }
}
