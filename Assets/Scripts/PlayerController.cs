using System.Collections; // Needed for IEnumerator
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator player_animator;
    private bool isReloading = false;
    private bool isShooting = false;
    private float reloadTime = 0.4f;
    private float shootCooldown = 0.3f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading && !isShooting && Player.ammo > 0)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetButtonDown("Reload") && !isReloading && !isShooting)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        player_animator.SetBool("Shooting", true);

        ShootRay();
        
        // Wait for shoot cooldown
        yield return new WaitForSeconds(shootCooldown);

        player_animator.SetBool("Shooting", false);
        isShooting = false;
        Player.ammo -= 1;


    }

    IEnumerator Reload()
    {
        isReloading = true;
        player_animator.SetBool("Reloading", true);
        
        // Wait for reload animation time
        yield return new WaitForSeconds(reloadTime);
        
        player_animator.SetBool("Reloading", false);
        isReloading = false;

        Player.ammo = 5;
    }

    void ShootRay(){
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2,
        Camera.main.pixelHeight/2, 0f));
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit))
        {
        Debug.Log("Hit: " + hit.collider.name);
        }
    }
}