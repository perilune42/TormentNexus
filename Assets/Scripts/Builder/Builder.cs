using UnityEngine;

public class Builder : MonoBehaviour
{
    public void SetNode(MapNode node)
    {
        this.node = node;
        node.onUnitEnter += (unit) =>
        {
            if (unit != null) CancelBuild();
        };
        PendingBuildDisplay.gameObject.SetActive(false);
        GameTick.onTick += TickBuild;
    }

    public bool Enabled = true;
    private MapNode node;
    private float usedResource;
    public Unit PendingBuild;
    [SerializeField] BuildingUnit PendingBuildDisplay;
    public int TimeRemaining;

    public bool CanBuild(Unit template)
    {
        return CanBuildAny() && template.Cost <= node.Owner.Resource.ResourceAmount;
    }
    public bool CanBuildAny()
    {
        return Enabled && PendingBuild == null && node.ContainedUnit == null && node.GarrisonHealth > 0;
    }

    public void BuildUnit(Unit template)
    {
        if (template == null)
        {
            Debug.LogError("tried to build null unit");
        }
        node.Owner.Resource.ConsumeResource(template.Cost);
        PendingBuild = template;
        usedResource = template.Cost;
        TimeRemaining = PendingBuild.BuildTime;
        PendingBuildDisplay.InitiateBuild(template, node.Owner);
        PendingBuildDisplay.SetBuildProgress(0);
        PendingBuildDisplay.gameObject.SetActive(true);
        
    }

    private void TickBuild()
    {
        if (PendingBuild != null)
        {
            TimeRemaining--;
            if (PendingBuildDisplay == null || PendingBuild == null)
            {
                Debug.LogError($"Build Error at {node.Name}");
            }
            PendingBuildDisplay.SetBuildProgress(1 - (float)TimeRemaining / PendingBuild.BuildTime);
            if (TimeRemaining <= 0)
            {
                FinishBuild();
            }
        }

    }

    private void CancelBuild()
    {
        node.Owner.Resource.ConsumeResource(-usedResource); // refund
        usedResource = 0;
        PendingBuild = null;
        PendingBuildDisplay.gameObject.SetActive(false);
    }

    private Unit FinishBuild()
    {
        Unit newUnit = GameObject.Instantiate(PendingBuild);
        PendingBuild = null;
        usedResource = 0;
        UnitController.Instance.SpawnUnit(newUnit, node, node.Owner);
        PendingBuildDisplay.gameObject.SetActive(false);
        return newUnit;
    }
}
