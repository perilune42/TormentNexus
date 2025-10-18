using UnityEngine;

public class Builder
{
    public Builder(MapNode node)
    {
        this.node = node;
        node.onUnitEnter += (unit) =>
        {
            if (unit != null) CancelBuild();
        };
    }

    private MapNode node;
    private float usedResource;
    public Unit PendingBuild;
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
        GameTick.onTick += TickBuild;
    }

    private void TickBuild()
    {
        TimeRemaining--;
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
    }

    private Unit FinishBuild()
    {
        Unit newUnit = GameObject.Instantiate(PendingBuild);
        PendingBuild = null;
        usedResource = 0;
        UnitController.Instance.SpawnUnit(newUnit, node, node.Owner);
        GameTick.onTick -= TickBuild;
        return newUnit;
    }
}
