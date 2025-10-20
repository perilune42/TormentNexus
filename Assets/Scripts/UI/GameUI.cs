using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public Canvas techTreeCanvas;
    public TMP_Text primedAbilityText;

    private void Awake()
    {
        instance = this;

        PlayerControl.Instance.onPrimeAbility += (Ability a) =>
        {
            if (a != null)
            {
                primedAbilityText.text = $"Primed {a.gameObject.name}";
            }
            else
            {
                primedAbilityText.text = "";
            }
        };
        // techTreeCanvas.enabled = false;
    }

    private void Start()
    {
        techTreeCanvas.enabled = false;
    }

    public void ToggleTechTree()
    {
        techTreeCanvas.enabled = !techTreeCanvas.enabled;
    }
}
