using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Type = 2;
        Health = 3;
        Speed = 4;
        RoF = 2f;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            if (canShoot)
            {
                //Fire();
                for (int i = 0; i < 4; i++)
                {
                    GameObject temp = Instantiate(Bullet, transform.position, Quaternion.identity);
                    temp.GetComponent<EnemyBullet>().speed = Speed;
                    temp.GetComponent<EnemyBullet>().transform.rotation = Quaternion.EulerAngles(0, 0, transform.rotation.z + (90 * Mathf.Deg2Rad * i) + 45 * Mathf.Deg2Rad);
                }
                canShoot = false;
                StartCoroutine(Reload());
            }

            if (Health <= 0)
            {
                levelManager.GetComponent<LevelManagerScript>().RemoveEnemy(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
