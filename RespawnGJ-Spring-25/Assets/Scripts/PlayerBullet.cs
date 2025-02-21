using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 7f;
    public PlayerController playerController; 

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
            Destroy(collision.gameObject);
            Destroy(gameObject); 

            if (playerController != null)
            {
                playerController.EnemyDefeated(); 
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the bullet on impact with wall
        }
    }
}
