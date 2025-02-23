using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : EnemyBase
{
    public GameObject levelManager;
    public GameObject PlayerRef;

    void Awake()
    {
        Type = 1;
        Health = 1;
        Speed = 6;
        started = false;
        canShoot = false;
        PlayerRef = GameObject.FindGameObjectWithTag("Player");

        // Get BPM from LevelManager
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            RoF = levelManager.GetComponent<LevelManagerScript>().beatInterval * 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
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
            Vector3 direction = PlayerRef.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert to degrees
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}
