using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static int ammo = 100000000;
    public static int health = 3;
    public GameObject weapon;
    public static Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
