using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        Debug.Log("Play Button Clicked");
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        foreach (var button in factionButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void OnQuitClick()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void OnStartClick()
    {
        FactionManager.startingFaction = chosen;
        Debug.Log("Game Start!");
        SceneManager.LoadScene("World", LoadSceneMode.Single);

        //player.faction = chosen;
    }
}
