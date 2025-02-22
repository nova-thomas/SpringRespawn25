using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    // Variables
    public int level;
    public int numE1;
    public int numE2;
    public int numE3;

    // References
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;
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

        level = 25;
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
            startNewLevel();
        }

        // Win Condition
        if (level == 25 && levelEnemies.Count == 0)
        {

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
        // Spawn Indicators (Change sprite to spawn indicators)
        foreach (GameObject enemy in levelEnemies)
        {
            // Pseudocode: Change enemy's sprite to spawn indicator sprite
            // enemy.GetComponent<SpriteRenderer>().sprite = spawnIndicatorSprite;
        }

        // Wait 5 seconds
        yield return new WaitForSeconds(5f);

        // Change Sprites to normal and set started to true
        foreach (GameObject enemy in levelEnemies)
        {
            // Pseudocode: Change sprite back to normal and set 'started' to true
            // enemy.GetComponent<SpriteRenderer>().sprite = normalSprite;
            // enemy.GetComponent<EnemyScript>().started = true;
        }
    }

    public void startNewLevel()
    {
        // Set number of enemies 
        // Min 1, if level 1-10, 1 per level, if level 11-30, 1 per 4 levels, 31-50, 1 per 6
        if(level <= 10)
        {
            numE1 = 1 + (level / 1) * 1;
        } else if (level <= 30) 
        {
            numE1 = 10 + (level / 4) * 1;
        } else if (level <= 50)
        {
            numE1 = 15 + (level / 6) * 1;
        }
        numE2 = 4 + (level / 5) * 1; // Min 3, adding 2 every 10 levels
        numE3 = 3 + (level / 15) * 3; // Min 3, adding 3 every 15 levels

        // Spawn Wave
        SpawnWave(numE1, numE2, numE3);
    }
}
