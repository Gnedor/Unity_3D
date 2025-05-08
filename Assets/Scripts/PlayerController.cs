using System.Collections; // Needed for IEnumerator
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Animator player_animator;
    private bool isReloading = false;
    private bool isShooting = false;
    public List<WeaponDatabase> weapons = new List<WeaponDatabase>();
    private WeaponDatabase currentWeapon;
    private int weaponIndex = 0;
    private int shotsFired = 0;
    public TextMeshProUGUI ammoCounter;
    LineRenderer shotLine;
    Transform weapon;
    float beamVisibleTime;
    void Start()
    {
        currentWeapon = weapons[weaponIndex];
        ammoCounter.text = (currentWeapon.maxAmmo - shotsFired) + "/" + currentWeapon.maxAmmo;
        shotLine = GetComponent<LineRenderer>();
        weapon = transform.Find("First Person Camera/Weapon");
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isReloading && !isShooting && Player.ammo > 0)
        {
            StartCoroutine(Shoot());
        ammoCounter.text = (currentWeapon.maxAmmo - shotsFired) + "/" + currentWeapon.maxAmmo;
        }

        if (Input.GetButtonDown("Reload") && !isReloading && !isShooting)
        {
            StartCoroutine(Reload());
        }

        beamVisibleTime -= Time.deltaTime;
        if (beamVisibleTime <= 0.0f){
            shotLine.enabled = false;
        }
    }

    IEnumerator Shoot()
    {
        if (shotsFired < currentWeapon.maxAmmo && Player.ammo > 0) {
            shotsFired += 1;
            isShooting = true;
            player_animator.SetBool("Shooting", true);

            ShootRay();
            
            // Wait for shoot cooldown
            yield return new WaitForSeconds(currentWeapon.fireRate);

            player_animator.SetBool("Shooting", false);
            isShooting = false;
            Player.ammo -= 1;
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
        ammoCounter.text = (currentWeapon.maxAmmo - shotsFired) + "/" + currentWeapon.maxAmmo;
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
                
                stats.GetHit(currentWeapon.damage);
            }
        }

        ShowRay(hit);
    }

    void ShowRay(RaycastHit hit){
        shotLine.SetPosition(0, weapon.position);
        shotLine.SetPosition(1, hit.point);
        shotLine.enabled = true;
        beamVisibleTime = 0.2f;
    }
}