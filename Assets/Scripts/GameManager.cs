using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void FactionVictory(Faction faction)
    {
        if (faction == FactionManager.instance.playerFaction)
        {
            Debug.Log("Player wins :D");
        }
        else
        {
            Debug.Log("Player loses :(");
        }

        // Do some cool slide in thing with victor's flag???
        // Show score or time taken I dunno
        // Exit button or just kick player to main menu
    }
}
