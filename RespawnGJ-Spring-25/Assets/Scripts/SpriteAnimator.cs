using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames; // Array of sprites to cycle through
    public float frameRate; // Time between frames in seconds

    private SpriteRenderer spriteRenderer;
    private int currentFrame;
    private float timer;
    public GameObject levelManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = 0f;
        levelManager = GameObject.FindGameObjectWithTag("LevelManager");
        frameRate = levelManager.GetComponent<LevelManagerScript>().beatInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length; // Loop back to 0
            spriteRenderer.sprite = frames[currentFrame]; // Set the current sprite
        }
    }
}
