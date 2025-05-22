using System;
using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public float speed;
    public bool dead = false;
    Rigidbody rb;
    private SpriteController spriteController;
    private SpriteRenderer sr;
    public bool follow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        spriteController = GetComponent<SpriteController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHit(int damage)
    {
        health -= damage;
        StartCoroutine(DamageFlash());
        follow = true;
        if (health <= 0 && !dead)
        {
            health = 0;
            Die();
            dead = true;
        }

    }

    void Die()
    {
        rb.constraints = RigidbodyConstraints.None;
        speed = 0;
        dead = true;
        float forceMagnitude = 500f;
        Vector3 forceDirection = Player.rb.transform.forward;
        Vector3 targetRotation = new Vector3(forceDirection.x, 0, forceDirection.z);

        rb.AddForce(targetRotation * forceMagnitude, ForceMode.Force);
        StartCoroutine(Remove());
    }

    IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    IEnumerator Remove()
    {
        float alpha = 1.0f;
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i <= 5; i++)
        {
            sr.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            alpha -= 0.2f;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
