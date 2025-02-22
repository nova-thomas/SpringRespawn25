using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Type = 2;
        Health = 3;
        Speed = 4;
        RoF = .75f;
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canShoot)
        {
            Fire();
        }

        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        transform.Rotate(0, 0, 90*Time.deltaTime);
    }
}
