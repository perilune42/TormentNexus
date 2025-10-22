using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text titleText;
    public Button playButton;
    public Button quitButton;
    public Button mongoliaButton;

    private void Start()
    {
        titleText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        mongoliaButton.gameObject.SetActive(false);
    }
    public void OnPlayClick()
    {
        Debug.Log("Play Button Clicked");
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        mongoliaButton.gameObject.SetActive(true);
    }

    public void OnQuitClick()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
