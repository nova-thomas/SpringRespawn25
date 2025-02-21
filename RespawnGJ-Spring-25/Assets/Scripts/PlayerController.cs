using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform frontPoint; // Assign this in the inspector
    public float fireRate = 0.2f;
    public Transform cameraTransform; // Assign the main camera transform
    public float cameraSmoothSpeed = 5f;

    private Vector2 moveDirection;
    private Camera mainCamera;
    private Coroutine shootingCoroutine;
    private bool isShooting;

    private void Start()
    {
        mainCamera = Camera.main;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
    }

    private void Update()
    {
        FollowMouse();
        FollowPlayerWithCamera();
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        moveDirection = (mousePosition - (Vector2)transform.position).normalized;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

        // Rotate towards movement direction
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
}