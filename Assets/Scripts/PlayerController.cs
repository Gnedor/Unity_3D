using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator player_animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }

    void shoot()
    {
        player_animator.SetTrigger("Shoot");
    }
}
