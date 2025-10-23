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
    public Button quitButton;
    public Button startButton;
    public List<FactionButton> factionButtons;
    public MainMenu instance;
    public Faction chosen;
    public AudioSource audioSource;
    public AudioClip UIButtonSFX;
    public AudioClip UIAcceptSFX;

    public GameObject FactionSelectScreen;


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
        audioSource.PlayOneShot(UIButtonSFX);
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        foreach (var button in factionButtons)
        {
            button.gameObject.SetActive(true);
        }
        FactionSelectScreen.SetActive(true);
    }

    public void OnQuitClick()
    {
        audioSource.PlayOneShot(UIButtonSFX);
        Application.Quit();
    }

    public void OnStartClick()
    {
        audioSource.PlayOneShot(UIAcceptSFX);
        FactionManager.startingFaction = chosen;
        SceneManager.LoadScene("World", LoadSceneMode.Single);
    }
}
