using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    public Faction Faction;
    private Dictionary<Unit, Assignment> unitAssignments = new();
    private int abilityBoostDays;
    private int restDays = 0;
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

    public void TempAbilityBoost()
    {
        abilityBoostDays = 4;
        restDays = 0;
    }

    private void ReassessStrategy()
    {
        if (restDays > 0)
        {
            restDays--;
        }
        else if (Faction.isMajorFaction)
        {
            restDays = Random.Range(3, 8);
            ChooseResearch();
            // chance per day to consider building units
            if (Random.value < 0.2)
            {
                // avoid clogging up
                if (Faction.AllUnits.Count < 5 || Faction.AllUnits.Count < Faction.AllNodes.Count * 0.6f)
                {
                    BuildUnits();
                }
                    
            }

            float buildAbilityChance = 0.2f;
            // if not at least 1 stockpiled, increase chance
            if (Faction.Abilities.Where((ability) => ability.CanLaunch()).Count() == 0)
            {
                buildAbilityChance *= 3f;
            }

            // chance to build a new weapon to stockpile
            if (Random.value < buildAbilityChance)
            {
                // if critically low on units, don't consider extra abilities
                if (buildAbilityChance > 0.5f || Faction.AllUnits.Count > 2)
                {
                    BuildAbility();
                }
            }

            if (Faction.Abilities.Where((ability) => ability.CanLaunch()).Count() > 0)
            {
                // chance to consider launching an ability specifically at the player due to hate
                float hatePercent = Faction.HateMeter.CurrentHate / HateMeter.MaxHate;
                float targetedLaunchChance = 0.2f * hatePercent;
                if (abilityBoostDays > 0) targetedLaunchChance *= 8f;
                if (Random.value < targetedLaunchChance)
                {
                    LaunchAbility(FactionManager.instance.playerFaction, frontierOnly: false);
                    abilityBoostDays--;
                }


                // chance to consider launching an ability at a random enemy
                float launchChance = 0.005f;
                if (WorldTension.Instance.CurrentTension > 0.1f)
                {
                    launchChance += Mathf.InverseLerp(0.1f, WorldTension.MaxTension, WorldTension.Instance.CurrentTension) * 0.2f;
                }
                if (Random.value < launchChance)
                {
                    var targetableFactions = FactionManager.instance.Factions;
                    targetableFactions.Remove(Faction);
                    LaunchAbility(targetableFactions.GetRandom(), true);
                }
            }
            
        }

        SetAssignments();
        if (abilityBoostDays > 0) abilityBoostDays--;
    }

    private void ChooseResearch()
    {
        // random bs go
        if (Faction.TechTree.currentlyResearching != null) return;
        var availableNodes = Faction.TechTree.techNodeStatuses.Keys.Where((node) => Faction.TechTree.techNodeStatuses[node] == TechNodeStatus.Unlocked);
        if (availableNodes.Count() == 0) return;
        Faction.TechTree.StartResearch(availableNodes.ToList().GetRandom());
    }

    private void BuildUnits()
    {
        // chance to keep building stuff until find one that it can't afford
        bool keepBuilding = true;
        List<MapNode> validBuildNodes = new List<MapNode>();
        foreach (MapNode node in Faction.AllNodes)
        {
            if (node.Builder.CanBuildAny())
            {
                validBuildNodes.Add(node);
            }
        }

        while (keepBuilding)
        {
            if (validBuildNodes.Count == 0) return;

            Unit targetUnit = Faction.BuildableUnits.GetRandom();
            if (targetUnit == null) Debug.LogError("Tried to build null unit");
            MapNode targetNode = validBuildNodes.GetRandom();
            if (targetNode.Builder.CanBuild(targetUnit))
            {
                targetNode.Builder.BuildUnit(targetUnit);
                validBuildNodes.Remove(targetNode);
                if (Random.value > 0.7f) keepBuilding = false; 
            }
            else
            {
                return;
            }
        }

    }

    private void BuildAbility()
    {
        Ability chosenAbility = Faction.Abilities.GetRandom();
        if (chosenAbility.CanBuild())
        {
            chosenAbility.BuildNew();
        }
    }

    private void LaunchAbility(Faction targetFaction, bool frontierOnly = false)
    {
        // choose a node, prefer one with units in it
        var targetableNodes = GetFrontierNodes().Where((node) => node.Owner == targetFaction);
        var preferredNodes = targetableNodes.Where((node) => node.ContainedUnit != null);
        if (preferredNodes.Count() == 0) preferredNodes = targetableNodes;
        if (preferredNodes.Count() == 0)
        {
            if (frontierOnly) return;
            targetableNodes = targetFaction.AllNodes;
            preferredNodes = targetableNodes.Where((node) => node.ContainedUnit != null);
            if (preferredNodes.Count() == 0) preferredNodes = targetableNodes;
            if (preferredNodes.Count() == 0) return;
        }

        var launchableAbilities = Faction.Abilities.Where((ability) => ability.CanLaunch()).ToList();
        if (launchableAbilities.Count() == 0) return;
        var chosenAbility = launchableAbilities.GetRandom();
        chosenAbility.Launch(preferredNodes.ToList().GetRandom());

    }

    private void SetAssignments()
    {
        foreach (Unit unit in Faction.AllUnits.Shuffled())
        {
            Assignment prevAssignment = unitAssignments[unit];
            Assignment newAssignment = prevAssignment;

            if (Random.value < 0.3f)
            {
                if (unit.Health < unit.MaxHealth * 0.2f)
                {
                    newAssignment = new RetreatAssignment(unit);
                }
            }
            if (Random.value < 0.2f)
            {
                if (unit.Health > unit.MaxHealth * 0.8f && prevAssignment is RetreatAssignment)
                {
                    newAssignment = null;
                }
            }
            unitAssignments[unit] = newAssignment;
        }

        if (Random.value < 0.2)
        {
            //randomly unassign defenders to reduce lockups
            foreach (var unit in Faction.AllUnits)
            {
                if (unitAssignments[unit] is DefendAssignment)
                {
                    if (Random.value < 0.5f)
                    {
                        unitAssignments[unit] = null;
                    }
                }
            }
        }

        int defenderQuota = (int)(0.2f * Faction.AllUnits.Count) + 1;
        int currentDefenders = unitAssignments.Values.Where((a) => a is DefendAssignment).Count();
        foreach (Unit unit in Faction.AllUnits.Shuffled())
        {

            if (unitAssignments[unit] == null)
            {
                if (Faction.isMajorFaction)
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
                else
                {
                    unitAssignments[unit] = new DefendAssignment(unit);
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

    // all enemy nodes adjacent to current territory
    private List<MapNode> GetFrontierNodes()
    {
        List<MapNode> frontier = new List<MapNode>();
        foreach (MapNode node in Faction.AllNodes)
        {
            foreach (MapNode neighbor in node.Neighbors)
            {
                if (neighbor.Owner != Faction && !frontier.Contains(neighbor))
                {
                    frontier.Add(neighbor);
                }
            }
        }
        return frontier;
    }
}



