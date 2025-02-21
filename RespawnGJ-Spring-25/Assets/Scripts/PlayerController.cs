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

    private void Start()
    {
        mainCamera = Camera.main;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
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

    private void UpdateUI()
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
    }
}