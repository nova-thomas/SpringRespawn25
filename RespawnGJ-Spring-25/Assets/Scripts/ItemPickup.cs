using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Transform player;
    private PlayerController playerController;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (player != null)
        {

            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController != null)
        {
            GiveRandomItem();
            Destroy(gameObject); 
        }
    }

    private void GiveRandomItem()
    {
        if (Random.value < 0.5f)
        {
            playerController.bombAmount++; // Give a bomb
        }
        else
        {
            playerController.shieldAmount++; // Give a shield
        }

        playerController.PlayItemPickupSound(); 
        playerController.UpdateUI(); 
    }
}
