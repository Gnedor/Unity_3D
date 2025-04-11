using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetHit(int damage){
        health -= damage;
        Debug.Log(health);
    }

    void Die(){
        
    }
}
