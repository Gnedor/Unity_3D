using UnityEngine;

public class SkelettController : MonoBehaviour
{
    private GameObject player;
    public float speed = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag ("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}

// gör så att freeze rotation x stängs av när de dör, gör så att de lägger sig ner
// gör så att de kan bli skjutna, gör så att de kan attackera
