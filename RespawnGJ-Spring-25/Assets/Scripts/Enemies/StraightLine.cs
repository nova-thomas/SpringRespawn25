using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : EnemyBase
{
    public GameObject Player;

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

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Vector3 direction = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x);
            transform.rotation = Quaternion.EulerAngles(0,0,angle-90*Mathf.Deg2Rad);
        }
    }
}
