using UnityEngine;
using UnityEngine.UI;

public class RelationshipInfo : MonoBehaviour
{
    Faction faction;
    [SerializeField] Image flagImage;
    [SerializeField] ProgressBar hateBar;

    public void SetFaction(Faction faction)
    {
        this.faction = faction;
        flagImage.sprite = faction.Flag;
    }

    private void Update()
    {
        hateBar.SetLevel(faction.HateMeter.CurrentHate / HateMeter.MaxHate);
        // if (!FactionManager.instance.RivalFactions.Contains(faction) && !FactionManager.instance.MinorFactions.Contains(faction))
        // {
        //     flagImage.color = Color.gray;
        // }
    }
}
