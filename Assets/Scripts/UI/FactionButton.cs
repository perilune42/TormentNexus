using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FactionButton : MonoBehaviour
{
    public Faction faction;
    public Image square;
    public TMP_Text factionName;
    public TMP_Text lore;
    public MainMenu menu;

    public void Awake()
    {
        //set in inspector
        square.color = faction.FactionColor;
        factionName.text = faction.FactionName;
        square.gameObject.SetActive(false);
        lore.gameObject.SetActive(false);
    }

    public void onClick()
    {
        menu.instance.startButton.gameObject.SetActive(true);
        foreach (var faction in menu.instance.factionButtons)
        {
            faction.square.gameObject.SetActive(false);
            faction.lore.gameObject.SetActive(false);
        }
        square.gameObject.SetActive(true);
        lore.gameObject.SetActive(true);
        menu.instance.chosen = faction; 
    }
}
