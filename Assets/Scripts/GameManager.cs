using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void FactionVictory(Faction faction)
    {
        GameTick.instance.SetSpeed(GameSpeed.Paused);
        GameTick.instance.enabled = false;

        GameUI.instance.ShowEndScreen(faction == FactionManager.instance.playerFaction);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
