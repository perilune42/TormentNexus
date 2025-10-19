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
    }

    private MapNode node;
    private float usedResource;
    public Unit PendingBuild;
    [SerializeField] BuildingUnit PendingBuildDisplay;
    public int TimeRemaining;

    public bool CanBuild(Unit template)
    {
        return PendingBuild == null && node.ContainedUnit == null && node.GarrisonHealth > 0 && template.Cost <= node.Owner.Resource.ResourceAmount;
    }
    public void BuildUnit(Unit template)
    {
        node.Owner.Resource.ConsumeResource(template.Cost);
        PendingBuild = template;
        usedResource = template.Cost;
        TimeRemaining = PendingBuild.BuildTime;
        PendingBuildDisplay.InitiateBuild(template, node.Owner);
        PendingBuildDisplay.SetBuildProgress(0);
        PendingBuildDisplay.gameObject.SetActive(true);
        GameTick.onTick += TickBuild;
    }

    private void TickBuild()
    {
        TimeRemaining--;
        PendingBuildDisplay.SetBuildProgress(1 - (float)TimeRemaining / PendingBuild.BuildTime);
        if (TimeRemaining <= 0)
        {
            FinishBuild();
        }
    }

    private void CancelBuild()
    {
        node.Owner.Resource.ConsumeResource(-usedResource); // refund
        usedResource = 0;
        PendingBuild = null;
        GameTick.onTick -= TickBuild;
        PendingBuildDisplay.gameObject.SetActive(false);
    }

    private Unit FinishBuild()
    {
        Unit newUnit = GameObject.Instantiate(PendingBuild);
        PendingBuild = null;
        usedResource = 0;
        UnitController.Instance.SpawnUnit(newUnit, node, node.Owner);
        GameTick.onTick -= TickBuild;
        PendingBuildDisplay.gameObject.SetActive(false);
        return newUnit;
    }
}
