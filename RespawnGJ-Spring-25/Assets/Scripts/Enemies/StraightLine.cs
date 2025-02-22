using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLine : EnemyBase
{
    public GameObject levelManager;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Type = 1;
        Health = 1;
        Speed = 6;
        started = false;
        canShoot = false;

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

            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                Vector3 direction = Player.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);
                transform.rotation = Quaternion.EulerAngles(0, 0, angle - 90 * Mathf.Deg2Rad);
            }
        }
    }
}
