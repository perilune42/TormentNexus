using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    public Faction Faction;

    public void SetToFaction(Faction faction)
    {
        this.Faction = faction;
        GameTick.onTick += Tick;
    }

    private void Tick()
    {
        foreach (Unit unit in Faction.AllUnits)
        {
            if (unit.MoveOrder != null) break;
            List<MapNode> neighboringEnemies = new();
            foreach (MapNode neighbor in unit.CurrentNode.Neighbors)
            {
                if (neighbor.Owner != Faction)
                {
                    neighboringEnemies.Add(neighbor);
                }
            }
            UnitController.Instance.MoveUnit(unit, neighboringEnemies.GetRandom());

        }
    }
}
