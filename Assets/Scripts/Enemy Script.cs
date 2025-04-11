using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints &= ~RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHit(int damage){
        health -= damage;
        if (health <= 0) 
            health = 0;
            Die();
    }

    void Die(){
        rb.constraints = RigidbodyConstraints.None;
    }
}
