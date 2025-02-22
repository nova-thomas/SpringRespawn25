using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        Type = 2;
        Health = 2;
        Speed = 4;
        started = false;
        canShoot = false;

        // Get BPM from LevelManager
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            RoF = levelManager.GetComponent<LevelManagerScript>().beatInterval;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (canShoot)
            {
                Fire();
            }

            if (Health <= 0)
            {
                levelManager.GetComponent<LevelManagerScript>().RemoveEnemy(gameObject);
                Destroy(gameObject);
            }

            transform.Rotate(0, 0, 90 * Time.deltaTime);
        }
    }
}
