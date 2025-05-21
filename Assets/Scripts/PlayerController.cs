using System.Collections; // Needed for IEnumerator
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEditor.Callbacks;
using UnityEngine.Animations;
using System;

public class PlayerController : MonoBehaviour
{
    public Animator player_animator;
    private bool isReloading = false;
    private bool isShooting = false;
    public List<WeaponDatabase> weapons = new List<WeaponDatabase>();
    private WeaponDatabase currentWeapon;
    private int weaponIndex = 0;
    public TextMeshProUGUI ammoCounter, healthCounter;
    Transform weapon;
    public GameObject bulletPrefab;
    private bool weapon2Unlock = false, weapon3Unlock = false;
    private float shieldTimer = 0f;

    void Start()
    {
        currentWeapon = weapons[weaponIndex];
        ammoCounter.text = (currentWeapon.currentClip) + "/" + currentWeapon.maxAmmo;
        weapon = transform.Find("First Person Camera/Weapon");
        SwitchWeapon();
    }
    void Update()
    {
        if (Input.GetButton("Fire1") && !isReloading && !isShooting && Player.ammo > 0)
        {
            StartCoroutine(Shoot());
            ammoCounter.text = (currentWeapon.currentClip) + "/" + currentWeapon.maxAmmo;
        }

        if (Input.GetButtonDown("Reload") && !isReloading && !isShooting)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponIndex = 0;
            SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapon2Unlock)
            {
                weaponIndex = 1;
                SwitchWeapon();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapon3Unlock)
            {
                weaponIndex = 2;
                SwitchWeapon();
            }
        }

        if (shieldTimer > 0f)
        {
            shieldTimer -= Time.deltaTime;
            Debug.Log(shieldTimer);
        }
    }

    IEnumerator Shoot()
    {
        if (currentWeapon.currentClip > 0 && Player.ammo > 0)
        {
            Debug.Log(currentWeapon.maxAmmo);
            currentWeapon.currentClip -= 1;
            isShooting = true;
            player_animator.SetBool("Shooting", true);

            ShootRay();
            StartCoroutine(SpawnBullet());
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

        currentWeapon.currentClip = currentWeapon.maxAmmo;
        ammoCounter.text = (currentWeapon.currentClip) + "/" + currentWeapon.maxAmmo;
    }

    void ShootRay()
    {
        Ray myRay = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2,
        Camera.main.pixelHeight / 2, 0f));
        RaycastHit hit;

        if (Physics.Raycast(myRay, out hit))
        {
            Debug.Log("Hit: " + hit.collider.tag);
            if (hit.collider.tag == "Enemy")
            {
                GameObject hitObject = hit.collider.gameObject;
                EnemyScript stats = hitObject.GetComponent<EnemyScript>();

                stats.GetHit(currentWeapon.damage);
            }
        }
    }
    IEnumerator SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, weapon.position, weapon.rotation);
        float forceMagnitude = 200f;
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = bullet.transform.forward;
        dir = -dir;

        bulletRb.AddForce(dir * forceMagnitude, ForceMode.Impulse);
        bulletRb.detectCollisions = false;

        yield return new WaitForSeconds(1.0f);
        Destroy(bullet);
    }

    void SwitchWeapon()
    {
        currentWeapon = weapons[weaponIndex];
        player_animator.SetInteger("weaponIndex", weaponIndex);
        Transform vapen = transform.Find("First Person Camera/Weapon");
        foreach (Transform child in vapen)
        {
            child.gameObject.SetActive(false);
        }
        vapen.GetChild(weaponIndex).gameObject.SetActive(true);
        ammoCounter.text = (currentWeapon.currentClip) + "/" + currentWeapon.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            TakeDamage();
        }

        if (other.tag == "Uzi")
        {
            weapon2Unlock = true;
            GameObject parent = other.gameObject;
            Destroy(parent);
            weaponIndex = 1;
            SwitchWeapon();
        }
        if (other.tag == "Rifle")
        {
            weapon3Unlock = true;
            GameObject parent = other.gameObject;
            Destroy(parent);
            weaponIndex = 2;
            SwitchWeapon();
        }
    }

    public void TakeDamage()
    {
        if (shieldTimer <= 0)
        {
            Player.health -= 1;
            healthCounter.text = "HP: " + (Player.health) + "/3";
            shieldTimer = 3f;
        }
    }
}