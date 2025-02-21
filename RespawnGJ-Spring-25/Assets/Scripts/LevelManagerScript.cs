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
    

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        //player = GameObject.FindGameObjectWithTag("Player");
        startNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEnemies.Count == 0)
        {
            level++;
            startNewLevel();
        }

        // Win Condition
        if (level == 50 && levelEnemies.Count == 0)
        {

        }
    }

    public void SpawnWave(int numEnemy1, int numEnemy2, int numEnemy3)
    {
        StartCoroutine(SpawnWaveRoutine(numEnemy1, numEnemy2, numEnemy3));
    }
    private IEnumerator SpawnWaveRoutine(int numEnemy1, int numEnemy2, int numEnemy3)
    {
        float screenLeft = -7f;
        float screenRight = 7f;
        float screenTop = 4f;
        float screenBottom = -4f;

        // Spawn Pattern 2 Columns
        float yCenter = (screenTop + screenBottom) / 2;
        int rowsPerColumn = numEnemy1 / 4;
        for (int i = 0; i < numEnemy1; i++)
        {
            int column = i % 4;
            int row = i / 4;
            float yPos = yCenter + (row - (rowsPerColumn / 2)) * 1.5f; // Spacing enemies vertically
            float xPos = Mathf.Lerp(screenLeft, screenRight, column / 3f); // Distribute across 4 columns
            Vector2 position = new Vector2(xPos, yPos);
            levelEnemies.Add(Instantiate(Enemy1, position, Quaternion.identity));
        }

        if (level >= 3)
        {
            // Spawn Pattern Box
            int sides = Mathf.Max(4, numEnemy2);
            float perimeter = 2 * ((screenRight - screenLeft - 2f) + (screenTop - screenBottom - 2f));
            float spacing = perimeter / sides;
            float distanceCovered = 0f;

            for (int i = 0; i < numEnemy2; i++)
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
            // Spawn Pattern Circle
            float radius = 4f;
            float angleStep = 360f / numEnemy3;
            Vector2 center = new Vector2((screenLeft + screenRight) / 2, (screenTop + screenBottom) / 2);
            float startAngle = 90f; // Start from above center

            for (int i = 0; i < numEnemy3; i++)
            {
                float angle = (startAngle + i * angleStep) * Mathf.Deg2Rad;
                Vector2 position = new Vector2(center.x + Mathf.Cos(angle) * radius, center.y + Mathf.Sin(angle) * radius);
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
        numE1 = 4 + (level / 8) * 4; // Min 4, 4 added every 7 waves
        numE2 = 2 + (level / 10) * 2; // Min 4, adding 2 every 5 waves
        numE3 = 3 + (level / 15) * 3; // Min 3, adding 3 every 15 waves

        // Spawn Wave
        SpawnWave(numE1, numE2, numE3);
    }
}
