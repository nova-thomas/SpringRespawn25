using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject creditsPanel;
    

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
    }

    public void OnPlay()
    {
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
