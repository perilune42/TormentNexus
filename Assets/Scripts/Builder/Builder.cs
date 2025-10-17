using UnityEngine;

public class Builder
{
    public Builder(MapNode node)
    {
        this.node = node;
    }

    private MapNode node;
    private float usedResource;
    public Unit PendingBuild;
    public int TimeRemaining;


    public void BuildUnit(Unit template)
    {
        node.Owner.Resource.ConsumeResource(template.Cost);
        PendingBuild = template;
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
        PendingBuild = null;
        GameTick.onTick -= TickBuild;
    }

    private Unit FinishBuild()
    {
        Unit newUnit = GameObject.Instantiate(PendingBuild);
        UnitController.Instance.SpawnUnit(newUnit, node, node.Owner);
        PendingBuild = null;
        GameTick.onTick -= TickBuild;
        return newUnit;
    }
}
