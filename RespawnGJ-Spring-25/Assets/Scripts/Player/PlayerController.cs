using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int level = 1;
    public int health = 10; 
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;

    public Transform frontPoint;
    public float fireRate = 0.2f;
    private Vector2 moveDirection;

    public Transform cameraTransform;
    public float cameraSmoothSpeed = 5f;
    private Camera mainCamera;
    private Coroutine shootingCoroutine;
    private bool isShooting;

    private int enemiesDefeated = 0;
    private int enemiesToNextLevel;
    public TMP_Text levelText;
    public Slider progressBar;

    public int shieldAmount = 0;
    public int bombAmount = 0;
    public GameObject bombPrefab;
    public GameObject shieldPrefab;
    public TMP_Text shieldText;
    public TMP_Text bombText;

    public AudioClip itemSound;
    public AudioClip bombCountdownSound;
    public AudioClip bombExplosionSound;
    public AudioClip deathSound; 

    private AudioSource audioSource;

    public GameObject levelManager;

    private SpriteRenderer spriteRenderer;

    [Header("Health UI")]
    public Image[] heartImages; 
    public Sprite fullHeart;
    public Sprite halfHeart;

    [Header("UI Canvases")]
    public GameObject deathScreenCanvas;
    public GameObject mainCanvas;
    private bool isDead = false;

    private void Start()
    {
        isDead = false;
        mainCamera = Camera.main;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (mainCanvas != null )
        {
            mainCanvas.SetActive(true);
        }

        if (rb != null)
        {
            rb.gravityScale = 0;
        }

        enemiesToNextLevel = level * 2;
        UpdateUI();
        UpdateHealthUI();

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(false); 
        }
    }

    private void Update()
    {
        UpdateUI();
        FollowMouse();
        FollowPlayerWithCamera();
        if (enemiesDefeated >= enemiesToNextLevel)
        {
            LevelUp();
        }
    }

    private void FollowMouse()
    {
        if (isDead) return;
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        moveDirection = (mousePosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FollowPlayerWithCamera()
    {
        if (cameraTransform != null)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, cameraTransform.position.z);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, cameraSmoothSpeed * Time.deltaTime);
        }
    }

  

public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(Shoot());
        }
        else if (context.canceled)
        {
            isShooting = false;
            if (shootingCoroutine != null)
            {
                StopCoroutine(shootingCoroutine);
            }
        }
    }

    private IEnumerator Shoot()
    {
        while (isShooting)
        {
            Instantiate(bulletPrefab, frontPoint.position, frontPoint.rotation);
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void OnBombPlace(InputAction.CallbackContext context)
    {
        if (context.started && bombAmount > 0)
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        Vector3 bombSpawnPosition = transform.position + (transform.right * 1.5f); 

        GameObject bomb = Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);

        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        if (bombRb != null)
        {
            bombRb.gravityScale = 0;
            bombRb.velocity = transform.right * 2f; 
        }

        bombAmount--;

        if (audioSource != null && bombCountdownSound != null)
        {
            audioSource.PlayOneShot(bombCountdownSound);
        }

        StartCoroutine(ExplodeBomb(bomb));
    }


    private IEnumerator ExplodeBomb(GameObject bomb)
    {
        yield return new WaitForSeconds(2f);

        if (audioSource != null && bombExplosionSound != null)
        {
            audioSource.volume = 0.45f;
            audioSource.PlayOneShot(bombExplosionSound);
            audioSource.volume = 0.75f;
        }

        float explosionRadius = 3f;
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(bomb.transform.position, explosionRadius);

        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if (enemy.GetComponent<EnemyBase>().started == true && enemy.name != "BossEnemy(Clone)")
                {
                    enemy.GetComponent<EnemyBase>().Health = 0;
                } else if (enemy.GetComponent<EnemyBase>().started == true && enemy.name == "BossEnemy(Clone)")
                {
                    enemy.GetComponent<EnemyBase>().Health -= 3;
                }
            }
        }
        Destroy(bomb);
    }

    public void OnShieldActivate(InputAction.CallbackContext context)
    {
        if (context.started && shieldAmount > 0)
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        GameObject shield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shield.transform.SetParent(transform);
        shieldAmount--;

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DeactivateShield(shield));
    }

    private IEnumerator DeactivateShield(GameObject shield)
    {
        yield return new WaitForSeconds(3f);

        GetComponent<Collider2D>().enabled = true;

        Destroy(shield);
    }

    public void EnemyDefeated() 
    {
        enemiesDefeated++;
    }

    private void LevelUp()
    {
        level = levelManager.GetComponent<LevelManagerScript>().level;
        enemiesDefeated = 0;
        enemiesToNextLevel = levelManager.GetComponent<LevelManagerScript>().levelEnemies.Count;
        if (level % 5 == 0)
        {
            health = 10;
            UpdateHealthUI();
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (levelText != null)
        {
            levelText.text = "Level " + level;
        }

        if (progressBar != null)
        {
            progressBar.maxValue = enemiesToNextLevel;
            progressBar.value = enemiesDefeated;
        }
        if (bombText != null)
        {
            bombText.text = bombAmount.ToString();
        }
        if (shieldText != null)
        {
            shieldText.text = shieldAmount.ToString();
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(DamageFlash());
        health -= 1;
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        isDead = true;

        Time.timeScale = 0f;

        if (mainCanvas != null)
        {
            mainCanvas.SetActive(false);
        }

        if (deathScreenCanvas != null)
        {
            deathScreenCanvas.SetActive(true);
            
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = (i + 1) * 2; 

            if (health >= heartValue)
            {
                heartImages[i].sprite = fullHeart;
                heartImages[i].enabled = true;
            }
            else if (health == heartValue - 1)
            {
                heartImages[i].sprite = halfHeart;
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false; 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            if (transform.Find("Shield(Clone)") == null)
            {
                TakeDamage(); 
            }

            Destroy(collision.gameObject); 
        }
    }

    public void PlayItemPickupSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(itemSound);
        }
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Change to red
            yield return new WaitForSeconds(0.1f); // Short delay (impact frame)
            spriteRenderer.color = Color.white; // Back to normal
        }
    }
}
