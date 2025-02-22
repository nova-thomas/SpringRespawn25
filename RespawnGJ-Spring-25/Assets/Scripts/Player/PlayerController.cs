using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int level = 1;
    public int health = 3;
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

    private AudioSource audioSource;
    private void Start()
    {
        mainCamera = Camera.main;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (rb != null)
        {
            rb.gravityScale = 0;
        }

        enemiesToNextLevel = level * 2;
        UpdateUI();

    }

    private void Update()
    {
        UpdateUI();
        FollowMouse();
        FollowPlayerWithCamera();
    }

    private void FollowMouse()
    {
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
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
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

        float explosionRadius = 2f;
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(bomb.transform.position, explosionRadius);

        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Destroy(enemy.gameObject);
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
        StartCoroutine(DeactivateShield(shield));
    }

    private IEnumerator DeactivateShield(GameObject shield)
    {
        yield return new WaitForSeconds(3f); 
        Destroy(shield); 
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;

        if (enemiesDefeated >= enemiesToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        enemiesDefeated = 0;
        enemiesToNextLevel = level * 2;
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

    public void PlayItemPickupSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(itemSound); 
        }
    }
}