using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //Universal Enemy Variables
    public int Type;
    public int Health;
    public float Speed;
    public float RoF;
    public bool canShoot = true;
    public GameObject Bullet;
    public GameObject Item;
    public bool started;

    // Boss
    public int numRing;

    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(RoF);
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (started)
        {
            if (collision.gameObject.tag == "PlayerBullet")
            {
                StartCoroutine(DamageFlash());
                Destroy(collision.gameObject);
                Health--;
            }
        }
    }

    public void Fire()
    {
        GameObject temp = Instantiate(Bullet, transform.position, Quaternion.identity);
        temp.GetComponent<EnemyBullet>().speed = Speed;
        temp.GetComponent<EnemyBullet>().transform.rotation = transform.rotation;
        canShoot = false;
        StartCoroutine(Reload());
    }

    void OnDestroy()
    {
        GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        levelManager.GetComponent<LevelManagerScript>().RemoveEnemy(gameObject);
        player.GetComponent<PlayerController>().EnemyDefeated();

        if (Random.value < 0.6f && Item != null)
        {
            Instantiate(Item, this.transform.position, Quaternion.identity);
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
