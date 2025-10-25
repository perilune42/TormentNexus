using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject TitleScreen;
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

    public Image TitleBG;
    public List<Sprite> titleSprites;
    bool onTitleScreen = true;
    int frame = 0;

    private void Start()
    {
        instance = this;
        TitleScreen.SetActive(true);

        foreach (var button in factionButtons)
        {
            button.gameObject.SetActive(false);
        }
        startButton.gameObject.SetActive(false);
        StartCoroutine(TitleAnimation());
    }
    public void OnPlayClick()
    {
        audioSource.PlayOneShot(UIButtonSFX);
        TitleScreen.gameObject.SetActive(false);
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

    IEnumerator TitleAnimation()
    {
        while (onTitleScreen)
        {
            TitleBG.sprite = titleSprites[frame];
            frame = (frame + 1) % 2;
            yield return new WaitForSeconds(0.4f);
        }
    }
}
