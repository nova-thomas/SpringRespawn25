using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
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
        transform.position = Movement() + StartPos;
    }

    public Vector2 Movement()
    {
        float newX = transform.position.x * speed * Time.deltaTime;
        float newY = transform.position.x * speed * Time.deltaTime;
        return new Vector2(newX, newY);
    }
}
