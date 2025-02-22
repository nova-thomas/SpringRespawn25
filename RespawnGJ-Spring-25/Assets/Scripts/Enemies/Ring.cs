using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : EnemyBase
{
    public GameObject levelManager;

    public float ringRadius = 2f;
    void Awake()
    {
        Type = 2;
        Health = 3;
        Speed = 4;
        started = false;
        canShoot = false;

        // Get BPM from LevelManager
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            RoF = levelManager.GetComponent<LevelManagerScript>().beatInterval * 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(started)
        {
            if (canShoot)
            {
                SpawnRingOfBullets();
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

    private void SpawnRingOfBullets()
    {
        float angleStep = 360f / 4; // Angle between each bullet
        for (int i = 0; i < 4; i++)
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
