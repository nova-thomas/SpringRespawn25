using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 7f;
    public PlayerController playerController;
    public GameObject itemPickupPrefab; 

    private void Start()
    {
        Destroy(gameObject, lifetime);
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (playerController != null)
            {
                playerController.EnemyDefeated();
            }

            //40% chance
            if (Random.value < 0.6f && itemPickupPrefab != null)
            {
                Instantiate(itemPickupPrefab, collision.transform.position, Quaternion.identity);
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
