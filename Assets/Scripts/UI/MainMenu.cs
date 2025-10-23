using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text titleText;
    public Button playButton;
    public AudioClip UIAccept;
    public AudioClip UI2;
    public AudioSource audioSource;
    public Button quitButton;
    public Button startButton;
    public List<FactionButton> factionButtons;
    public MainMenu instance;
    public Faction chosen;


    private void Start()
    {
        instance = this;
        titleText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        foreach (var button in factionButtons)
        {
            button.gameObject.SetActive(false);
        }
        startButton.gameObject.SetActive(false);
    }
    public void OnPlayClick()
    {
        audioSource.PlayOneShot(UIAccept);
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        foreach (var button in factionButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void OnQuitClick()
    {
        audioSource.PlayOneShot(UIAccept);
        Application.Quit();
    }

    public void OnStartClick()
    {
        audioSource.PlayOneShot(UIAccept);
        FactionManager.startingFaction = chosen;
        SceneManager.LoadScene("World", LoadSceneMode.Single);
    }
}
