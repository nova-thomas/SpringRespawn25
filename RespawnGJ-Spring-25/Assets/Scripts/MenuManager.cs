using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;

    public AudioClip menuMusic;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false); // Ensure credits panel is hidden at start
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(true); // Ensure credits panel is hidden at start
        }

        // Initialize and play menu music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.Play();
    }

    public void OnPlay()
    {
        audioSource.Stop();
        SceneManager.LoadScene("Game");
    }

    public void OnCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(!creditsPanel.activeSelf); // Toggle credits panel
        }
        if (menuPanel != null)
        {
            menuPanel.SetActive(!menuPanel.activeSelf); // Toggle Menu panel
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
