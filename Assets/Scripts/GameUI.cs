using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public Canvas techTreeCanvas;

    private void Awake()
    {
        instance = this;
    }

    public void ToggleTechTree()
    {
        techTreeCanvas.enabled = !techTreeCanvas.enabled;
    }
}
