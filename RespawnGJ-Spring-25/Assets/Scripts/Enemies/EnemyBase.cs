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
        if (levelManager != null)
        {
            levelManager.GetComponent<LevelManagerScript>().RemoveEnemy(gameObject);
        }
    }
}
