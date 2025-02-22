using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float lifetime = 5f;
    public float timer = 0f;
    public Vector2 StartPos;

    public int bpm;
    private float beatInterval;
    private Transform spriteChild;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        StartPos = new Vector2(transform.position.x, transform.position.y);

        // Get BPM from LevelManager
        GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        if (levelManager != null)
        {
            beatInterval = levelManager.GetComponent<LevelManagerScript>().beatInterval;
        }

        // Get the child sprite transform
        spriteChild = transform.GetChild(0);

        // Start rotating on beat
        StartCoroutine(RotateOnBeat());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    public Vector2 Movement(float timer)
    {
        float newX = transform.up.x * speed * timer;
        float newY = transform.up.y * speed * timer;
        return new Vector2(newX+StartPos.x, newY+StartPos.y);
    }

    private IEnumerator RotateOnBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatInterval);
            Debug.Log($"Rotating sprite! Beat Interval: {beatInterval}s");
            spriteChild.Rotate(0, 0, 90f); // Rotate 90 degrees around the Z-axis
        }
    }
}
