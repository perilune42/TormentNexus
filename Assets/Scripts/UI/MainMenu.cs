using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TMP_Text titleText;
    public Button playButton;
    public Button quitButton;
    public Button mongoliaButton;
    public Button mingButton;
    public Button austriaButton;
    public Button eafButton;
    public Faction mongoliaFaction;
    public Faction mingFaction;
    public Faction austriaFaction;
    public Faction eafFaction;


    private void Start()
    {
        titleText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        mongoliaButton.gameObject.SetActive(false);
        austriaButton.gameObject.SetActive(false);
        mingButton.gameObject.SetActive(false);
        eafButton.gameObject.SetActive(false);
        mongoliaButton.GetComponentInChildren<TMP_Text>().text = mongoliaFaction.FactionName;
        austriaButton.GetComponentInChildren<TMP_Text>().text = austriaFaction.FactionName;
        mingButton.GetComponentInChildren<TMP_Text>().text = mingFaction.FactionName;
        eafButton.GetComponentInChildren<TMP_Text>().text = eafFaction.FactionName;
    }
    public void OnPlayClick()
    {
        Debug.Log("Play Button Clicked");
        titleText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        mongoliaButton.gameObject.SetActive(true);
        austriaButton.gameObject.SetActive(true);
        mingButton.gameObject.SetActive(true);
        eafButton.gameObject.SetActive(true);
    }

    public void OnQuitClick()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void OnMongoliaClick()
    {
        Debug.Log("Mongolia Selected");

    }

    public void OnMingClick()
    {
        Debug.Log("Ming Selected");
    }

    public void OnAustriaClick()
    {
        Debug.Log("Austria Selected");
    }

    public void OnEAFClick()
    {
        Debug.Log("EAF Selected");
    }
}
