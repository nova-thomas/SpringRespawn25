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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(RoF);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Health--;
        }
    }
}
