using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    public Faction Faction;
    private Dictionary<Unit, Assignment> unitAssignments = new();

    public void SetToFaction(Faction faction)
    {
        this.Faction = faction;
        GameTick.onTick += Tick;
        GameTick.onDay += ReassessStrategy;

        UnitController.Instance.OnAddUnit += (Unit unit) =>
        {
            if (unit.Owner == Faction)
            {
                unitAssignments.Add(unit, null);
                unit.OnMoved += (_) => ExecuteAssignment(unit);
                ExecuteAssignment(unit);
            }
        };
        UnitController.Instance.OnRemoveUnit += (Unit unit) =>
        {
            if (unit.Owner == Faction)
            {
                unitAssignments.Remove(unit);
            }
        };


        // SetAssignments();
    }

    private void Tick()
    {

    }

    private void ReassessStrategy()
    {
        SetAssignments();
    }

    private void SetAssignments()
    {
        foreach (Unit unit in Faction.AllUnits.Shuffled())
        {
            Assignment prevAssignment = unitAssignments[unit];
            Assignment newAssignment = prevAssignment;
            if (unit.Health < unit.MaxHealth * 0.2)
            {
                newAssignment = new RetreatAssignment(unit);
            }
            if (unit.Health > unit.MaxHealth * 0.9 && prevAssignment is RetreatAssignment)
            {
                newAssignment = null;
            }
            unitAssignments[unit] = newAssignment;
        }
        int defenderQuota = (int)(0.2f * Faction.AllUnits.Count) + 1;
        int currentDefenders = unitAssignments.Values.Where((a) => a is DefendAssignment).Count();
        foreach (Unit unit in Faction.AllUnits.Shuffled())
        {

            if (unitAssignments[unit] == null)
            {
                if (currentDefenders < defenderQuota)
                {
                    unitAssignments[unit] = new DefendAssignment(unit);
                    currentDefenders++;
                } 
                else if (Random.value < 0.5f)
                {
                    unitAssignments[unit] = new CaptureAssignment(unit);
                }
                else
                {
                    unitAssignments[unit] = new EliminateAssignment(unit);
                }
            }
            ExecuteAssignment(unit);
        }
        
    }

    private void ExecuteAssignment(Unit unit)
    {
        var assignment = unitAssignments[unit];

        if (unit.MoveOrder != null)
        {
            bool collidingWithFriendly = unit.MoveOrder.ContainedUnit != null && unit.MoveOrder.ContainedUnit.Owner == unit.Owner;
            if (!collidingWithFriendly && !assignment.Interruptable) return;
        }

        MapNode target = null;
        if (assignment != null) {
            target = assignment.GetNextMove();
        }

        if (target == null)
        {
            if (unit.MoveOrder == null) return;
            // Debug.Log($"AI ordering move to: {unit.CurrentNode}");
            UnitController.Instance.MoveUnit(unit, unit.CurrentNode);
        }
        else if (target != unit.MoveOrder)
        {
            // Debug.Log($"AI ordering move to: {target}");
            UnitController.Instance.MoveUnit(unit, target);
        }
    }
}



