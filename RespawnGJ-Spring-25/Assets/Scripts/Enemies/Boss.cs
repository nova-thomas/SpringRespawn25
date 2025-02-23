using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    public GameObject PlayerRef;

    public GameObject levelManager;

    public float ringRadius = 2f;

    public bool ringBool;
    void Awake()
    {
        Type = 3;
        Health = 10;
        Speed = 3;
        started = false;
        canShoot = false;
        PlayerRef = GameObject.FindGameObjectWithTag("Player");



        // Get BPM from LevelManager
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            RoF = 4 * levelManager.GetComponent<LevelManagerScript>().beatInterval * 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (canShoot)
            {
                // Ring
                if (ringBool)
                {
                    SpawnRingOfBullets();
                    ringBool = !ringBool;
                }
                
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
    private void SpawnRingOfBullets()
    {
        float angleStep = 360f / numRing; // Angle between each bullet
        for (int i = 0; i < numRing; i++)
        {
            // Calculate the position of each bullet in the ring
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert to radians
            Vector2 bulletPosition = new Vector2(
                transform.position.x + Mathf.Cos(angle) * ringRadius,
                transform.position.y + Mathf.Sin(angle) * ringRadius
            );

            // Instantiate the bullet
            GameObject temp = Instantiate(Bullet, bulletPosition, Quaternion.identity);
            EnemyBullet bulletScript = temp.GetComponent<EnemyBullet>();
            bulletScript.speed = Speed;

            // Set the bullet's rotation to face outward
            float bulletAngle = Mathf.Atan2(bulletPosition.y - transform.position.y, bulletPosition.x - transform.position.x) * Mathf.Rad2Deg;
            temp.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);
        }
    }
}
