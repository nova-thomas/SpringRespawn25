using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Type = 1;
        Health = 3;
        Speed = 6;
        RoF = 1f;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            GameObject temp = Instantiate(Bullet, transform.position, Quaternion.identity);
            temp.GetComponent<EnemyBullet>().speed = Speed;
            temp.GetComponent<EnemyBullet>().transform.rotation = transform.rotation;
            canShoot = false;
            StartCoroutine(Reload());
        }
    }
}
