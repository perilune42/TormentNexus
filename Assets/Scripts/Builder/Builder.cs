using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Unit BuildUnit(Faction faction, MapNode node, Unit template)
    {
        faction.Resource.ConsumeResource(template.Cost);
        Unit newUnit = Instantiate(template);
        UnitController.Instance.SpawnUnit(newUnit, node, faction);
        return newUnit;
    }
}
