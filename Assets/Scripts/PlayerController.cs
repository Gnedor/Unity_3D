using System.Collections; // Needed for IEnumerator
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator player_animator;
    private bool isReloading = false;
    private bool isShooting = false;
    public List<WeaponDatabase> weapons = new List<WeaponDatabase>();
    private WeaponDatabase currentWeapon;
    private int weaponIndex = 0;
    private int shotsFired = 0;

    void Start()
    {
        currentWeapon = weapons[weaponIndex];
    }
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
        Debug.Log("Gaming");
        if (shotsFired >= currentWeapon.maxAmmo || Player.ammo > 0) {
            isShooting = true;
            player_animator.SetBool("Shooting", true);

            ShootRay();
            
            // Wait for shoot cooldown
            yield return new WaitForSeconds(currentWeapon.fireRate);

            player_animator.SetBool("Shooting", false);
            isShooting = false;
            Player.ammo -= 1;
            shotsFired += 1;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        player_animator.SetBool("Reloading", true);
        
        // Wait for reload animation time
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        
        player_animator.SetBool("Reloading", false);
        isReloading = false;

        Player.ammo = 5;
        shotsFired = 0;
    }

    void ShootRay(){
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2,
        Camera.main.pixelHeight/2, 0f));
        RaycastHit hit;

        if(Physics.Raycast(myRay, out hit))
        {
            Debug.Log("Hit: " + hit.collider.tag);
            if (hit.collider.tag == "Enemy"){
                GameObject hitObject = hit.collider.gameObject;
                EnemyScript stats = hitObject.GetComponent<EnemyScript>();
                

                if (stats.health > 0){
                    stats.health -= currentWeapon.damage;
                    if (stats.health < 0) {
                        stats.health = 0;
                    }
                }
            }
        }
    }
}