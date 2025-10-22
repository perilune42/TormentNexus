using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameTick.instance.enabled = false;
    }
    public void OnPlayClick()
    {
        gameObject.SetActive(false);
        Debug.Log("Game Started");
        GameTick.instance.enabled = true;
    }

    public void OnQuitClick()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
