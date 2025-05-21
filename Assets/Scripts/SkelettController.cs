using UnityEngine;

public class SkelettController : MonoBehaviour
{
    private GameObject player;
    private EnemyScript enemyScript;
    private float detectionRadius = 3f;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            anim.SetTrigger("Attack");
        }
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);
    }
}

// gör så att freeze rotation x stängs av när de dör, gör så att de lägger sig ner
// gör så att de kan bli skjutna, gör så att de kan attackera
