using UnityEngine;
using UnityEngine.UI;

public class BuildingUnit : MonoBehaviour
{
    public ProgressBar BuildProgress;
    [SerializeField] Image icon;

    public void InitiateBuild(Unit unit, Faction faction)
    {
        icon.color = Color.Lerp(faction.FactionColor, Color.black, 0.6f);
        icon.sprite = unit.Icon;
    }

    public void SetBuildProgress(float level)
    {
        BuildProgress.SetLevel(level);
    }
}
