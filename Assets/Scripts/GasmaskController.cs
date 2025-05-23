using UnityEngine;
public class GasmaskController : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float shootingRadius = 2f;
    public float fireRate = 1.5f; // Time in seconds between shots
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public int maxAmmoCount = 30; // Maximum bullets before reload
    public float reloadTime = 2.0f; // Time it takes to reload (should match animation length)

    private GameObject player;
    private Animator animator;
    private float nextFireTime = 0f;
    private bool isReadyToFire = false;
    private bool isFiringSequence = false;
    private bool isReloading = false;
    private int currentAmmo;
    private EnemyScript enemyScript;

    // Animation event names
    private const string ANIM_TRIGGER_SHOOT = "shoot";
    private const string ANIM_TRIGGER_RELOAD = "reload";
    private const string ANIM_BOOL_WALKING = "isWalking";
    private const string ANIM_BOOL_IN_RANGE = "isInRange";
    private const string ANIM_EVENT_READY_TO_FIRE = "ReadyToFire"; // Add this to GMGunFire animation
    private const string ANIM_EVENT_RELOAD_COMPLETE = "ReloadComplete"; // Add this to GMReload animation

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmoCount; // Start with a full clip
        enemyScript = GetComponent<EnemyScript>();
    }

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= shootingRadius)
        {
            animator.SetBool(ANIM_BOOL_WALKING, false);
            animator.SetBool(ANIM_BOOL_IN_RANGE, true);
            FacePlayer();

            // Only start the shooting sequence if:
            // 1. Not already firing
            // 2. Not reloading
            // 3. Enough time has passed since last shot
            // 4. Has ammo
            if (!isFiringSequence && !isReloading && Time.time >= nextFireTime)
            {
                if (currentAmmo <= 0)
                {
                    StartReloading();
                }
                else
                {
                    StartFiringSequence();
                }
            }
        }

        else if (distanceToPlayer <= detectionRadius || enemyScript.follow)
        {
            enemyScript.follow = true;
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyScript.speed * Time.deltaTime);
            animator.SetBool(ANIM_BOOL_WALKING, true);
            animator.SetBool(ANIM_BOOL_IN_RANGE, false);
            FacePlayer();

            // Reset firing sequence if player moves out of range
            ResetFiringState();
        }
        else
        {
            animator.SetBool(ANIM_BOOL_WALKING, false);
            animator.SetBool(ANIM_BOOL_IN_RANGE, false);

            // Reset firing sequence if player moves out of range
            ResetFiringState();
        }
    }

    void FacePlayer()
    {
        Vector3 scale = transform.localScale;
        if (player.transform.position.x < transform.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void StartFiringSequence()
    {
        // Start the firing sequence
        isFiringSequence = true;
        isReadyToFire = false;

        // Trigger the shooting animation - this will play the pullout animation first
        animator.SetTrigger(ANIM_TRIGGER_SHOOT);

        // We don't call ShootAtPlayer() here - it will be called by the animation event
    }

    // This should be called by an animation event on the GMGunFire animation
    public void ReadyToFire()
    {
        isReadyToFire = true;
        ShootAtPlayer();
    }

    void ShootAtPlayer()
    {
        // Only shoot if ready
        if (!isReadyToFire || currentAmmo <= 0)
            return;

        if (bulletPrefab != null && firePoint != null)
        {
            // Calculate direction to the player
            Vector3 direction = (player.transform.position - firePoint.position).normalized;

            // Create the bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Use the Bullet script to set direction
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction * bulletSpeed);
            }

            // Optional: Flip bullet sprite if needed based on direction
            
            if (direction.x < 0)
            {
                Vector3 scale = bullet.transform.localScale;
                scale.x = -Mathf.Abs(scale.x);
                bullet.transform.localScale = scale;
            }
            

            // Decrease ammo count
            currentAmmo--;

            // Debug ammo count
            Debug.Log("Bullets remaining: " + currentAmmo);
        }

        // Reset for the next firing sequence
        nextFireTime = Time.time + fireRate;
        ResetFiringState();

        // If out of ammo, start reloading
        if (currentAmmo <= 0)
        {
            StartReloading();
        }
    }

    private void ResetFiringState()
    {
        isFiringSequence = false;
        isReadyToFire = false;
    }

    // Optional: This can be called at the end of the shooting animation sequence
    public void EndFiringSequence()
    {
        ResetFiringState();
    }

    // Start the reload process
    void StartReloading()
    {
        if (isReloading)
            return;

        isReloading = true;
        animator.SetTrigger(ANIM_TRIGGER_RELOAD);

        // We'll rely on the animation event to complete the reload
        Debug.Log("Reloading started...");
    }

    // Called by animation event at the end of the reload animation
    public void ReloadComplete()
    {
        currentAmmo = maxAmmoCount;
        isReloading = false;
        nextFireTime = Time.time; // Allow firing immediately after reload
        Debug.Log("Reload complete! Ammo refilled to " + currentAmmo);

        // If still in range, start firing again
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= shootingRadius)
        {
            StartFiringSequence();
        }
    }
}