using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float lifetime = 5f;
    public float timer = 0f;
    public Vector2 StartPos;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        StartPos = new Vector2(transform.position.x, transform.position.y);
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
}
