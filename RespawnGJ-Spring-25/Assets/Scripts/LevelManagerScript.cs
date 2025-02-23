using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    // Variables
    public int level;
    public int numE1;
    public int numE2;
    public int numE3;

    public int bpm = 110;
    public float beatInterval;

    // References
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public GameObject BossEnemy;
    public List<GameObject> levelEnemies = new List<GameObject>();

    public GameObject player;

    public AudioClip backgroundMusic;

    private AudioSource audioSource;
    


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
        audioSource.pitch = 0.5f;
        audioSource.volume = 0.25f;
        beatInterval = (60f / bpm) / audioSource.pitch;

        level = 1;
        player = GameObject.FindGameObjectWithTag("Player");

        startNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEnemies.Count == 0)
        {
            level++;
            audioSource.pitch += 0.04f;
            beatInterval = (60f / bpm) / audioSource.pitch;
            startNewLevel();
        }


        // Win Condition
        if (level == 25 && levelEnemies.Count == 0)
        {
            SceneManager.LoadScene("WinScreen");
        }
    }

    public void SpawnWave(int numEnemy1, int numEnemy2, int numEnemy3)
    {
        StartCoroutine(SpawnWaveRoutine(numEnemy1, numEnemy2, numEnemy3));
    }
    private IEnumerator SpawnWaveRoutine(int numEnemy1, int numEnemy2, int numEnemy3)
    {
        float screenLeft = -20f;
        float screenRight = 20f;
        float screenTop = 11f;
        float screenBottom = -11f;

        if (level % 5 != 0)
        {
            // Spawn Pattern Sprial
            float angleStepSprial = 35f; // Angle increment per enemy
            float radiusStep = 0.3f; // Distance increment per enemy
            Vector2 centerSprial = new Vector2((screenLeft + screenRight) / 2, (screenTop + screenBottom) / 2);
            float currentAngle = 90f;
            float currentRadius = 5f;

            for (int i = 0; i < numEnemy1; i++)
            {

                float angle = currentAngle * Mathf.Deg2Rad;
                Vector2 position = new Vector2(
                    centerSprial.x + Mathf.Cos(angle) * currentRadius,
                    centerSprial.y + Mathf.Sin(angle) * currentRadius
                );

                levelEnemies.Add(Instantiate(Enemy1, position, Quaternion.identity));

                currentAngle += angleStepSprial;
                currentRadius += radiusStep;
            }

            if (level >= 3)
            {
                // Spawn Pattern Box
                int sides = Mathf.Max(4, (numEnemy2 / 4) * 4); // Ensure divisible by 4
                float perimeter = 2 * ((screenRight - screenLeft - 2f) + (screenTop - screenBottom - 2f));
                float spacing = perimeter / sides;
                float distanceCovered = 0f;

                for (int i = 0; i < sides; i++) // Loop through corrected sides
                {
                    float pos = distanceCovered % perimeter;
                    Vector2 position;

                    if (pos < (screenRight - screenLeft - 2f)) // Top side
                    {
                        position = new Vector2(screenLeft + 1f + pos, screenTop - 1f);
                    }
                    else if (pos < (screenRight - screenLeft - 2f) + (screenTop - screenBottom - 2f)) // Right side
                    {
                        position = new Vector2(screenRight - 1f, screenTop - 1f - (pos - (screenRight - screenLeft - 2f)));
                    }
                    else if (pos < 2 * (screenRight - screenLeft - 2f) + (screenTop - screenBottom - 2f)) // Bottom side
                    {
                        position = new Vector2(screenRight - 1f - (pos - (screenRight - screenLeft - 2f + screenTop - screenBottom - 2f)), screenBottom + 1f);
                    }
                    else // Left side
                    {
                        position = new Vector2(screenLeft + 1f, screenBottom + 1f + (pos - (2 * (screenRight - screenLeft - 2f) + (screenTop - screenBottom - 2f))));
                    }

                    levelEnemies.Add(Instantiate(Enemy2, position, Quaternion.identity));
                    distanceCovered += spacing;
                }
            }

            if (level >= 5)
            {
                float waveAmplitude = 10f; // Height of the wave
                float waveFrequency = 1.5f; // Number of wave cycles across the screen
                float waveLength = screenRight - screenLeft; // Full horizontal span

                for (int i = 0; i < numEnemy3; i++)
                {
                    float progress = (float)i / (numEnemy3 - 1); // Normalized position between 0 and 1
                    float xPos = screenLeft + progress * waveLength; // Spread across the screen

                    // Alternate enemies in opposite wave patterns
                    float yPos = Mathf.Sin(progress * waveFrequency * Mathf.PI * 2) * waveAmplitude + (screenTop + screenBottom) / 2;
                    if (i % 2 == 1)
                    {
                        yPos = Mathf.Sin(progress * waveFrequency * Mathf.PI * 2 + Mathf.PI) * waveAmplitude + (screenTop + screenBottom) / 2; // Inverted wave
                    }

                    Vector2 position = new Vector2(xPos, yPos);
                    levelEnemies.Add(Instantiate(Enemy3, position, Quaternion.identity));
                }
            }
        } else // Boss Levels
        {
            Vector2 bossSpawnPosition = new Vector2(0f, 5f); // Centered spawn
            GameObject boss = Instantiate(BossEnemy, bossSpawnPosition, Quaternion.identity);
            levelEnemies.Add(boss);

            // Start boss behavior
            EnemyBase script = boss.GetComponent<Boss>();
            script.started = true;
            script.numRing = 4 + (level / 5);
            script.Speed = 3 + (level / 5);
            script.Health = 10 + (level / 5) * 5;
        }


        // Spawn Indicators (Enable indicator sprites)
        foreach (GameObject enemy in levelEnemies)
        {
            SpriteRenderer[] renderers = enemy.GetComponentsInChildren<SpriteRenderer>(true);

            if (renderers.Length > 1) // Assuming the first is the enemy, second is the indicator
            {
                renderers[0].enabled = false; // Disable main enemy sprite
                renderers[1].enabled = true;  // Enable spawn indicator
            }
        }

        // Wait 5 seconds
        yield return new WaitForSeconds(5f);

        // Re-enable main sprites and disable indicators
        foreach (GameObject enemy in levelEnemies)
        {
            SpriteRenderer[] renderers = enemy.GetComponentsInChildren<SpriteRenderer>(true);

            if (renderers.Length > 1)
            {
                renderers[1].enabled = false; // Disable spawn indicator
                renderers[0].enabled = true;  // Enable main enemy sprite
            }

            // Start enemy behavior
            enemy.GetComponent<EnemyBase>().started = true;
            enemy.GetComponent<EnemyBase>().canShoot = true;
        }
    }

    public void startNewLevel()
    {
        // Set number of enemies 
        // Min 1, if level 1-10, 1 per level, if level 11-30, 1 per 4 levels, 31-50, 1 per 6
        if(level <= 10)
        {
            numE1 = 0 + (level / 1) * 1;
        } else if (level <= 20) 
        {
            numE1 = 10 + (level / 3) * 1;
        } else if (level <= 25)
        {
            numE1 = 13 + (level-20) / 2;
        }
        numE2 = 2 + (level / 4) * 1; // Min 2, adding 1 every 4 levels
        numE3 = 2 + (level / 5) * 1; // Min 2, adding 1 every 5 levels

        // Spawn Wave
        SpawnWave(numE1, numE2, numE3);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (levelEnemies.Contains(enemy))
        {
            levelEnemies.Remove(enemy);
        }
    }
}
