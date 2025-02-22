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
    public GameObject levelManager;

    public int bpm;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        canShoot = false;

        // Get BPM from LevelManager
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            RoF = levelManager.GetComponent<LevelManagerScript>().beatInterval;
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(RoF);
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(started == true)
        {
            if (collision.gameObject.tag == "PlayerBullet")
            {
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
        if (levelManager != null)
        {
            levelManager.GetComponent<LevelManagerScript>().RemoveEnemy(gameObject);
        }
    }
}
