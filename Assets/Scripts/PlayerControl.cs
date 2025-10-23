using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public MapNode HoveredNode;
    public MapNode SelectedNode;
    public Unit SelectedUnit;
    public Action<MapNode> onSelectNode;
    public Action<Unit> onSelectUnit;
    public Action<Ability> onPrimeAbility;

    Ability primedAbility;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SelectUnit(null);
            SelectNode(null);
        } 
    }

    public void SelectNode(MapNode node)
    {
        if (primedAbility != null)
        {
            if (node != null)
            {
                LaunchAbility(primedAbility, node);
                return;
            }
        }

        if (SelectedNode != null) SelectedNode.ToggleSelectHighlight(false);
        SelectedNode = node;
        onSelectNode?.Invoke(node);
        if (node != null)
        {
            node.ToggleSelectHighlight(true);
            SelectUnit(null);
        }
        BuildMenu.Instance.UpdateBuildables();
    }


    public void HoverNode(MapNode node)
    {
        HoveredNode = node;
    }

    public void RClickNode(MapNode node)
    {
        if (SelectedUnit != null) 
        {
            if (SelectedUnit.Owner != FactionManager.instance.playerFaction) return;

            if (node == SelectedUnit.CurrentNode || UnitController.Instance.IsValidMove(SelectedUnit, node, ignoreOccupied:true))
            {
                UnitController.Instance.MoveUnit(SelectedUnit, node);
            }
        }
    }

    public void SelectUnit(Unit unit)
    {
        if (primedAbility != null)
        {
            if (unit != null)
            {
                LaunchAbility(primedAbility, unit.CurrentNode);
                return;
            }
        }
        if (SelectedUnit != null) SelectedUnit.Display.ToggleSelectHighlight(false);
        SelectedUnit = unit;
        onSelectUnit?.Invoke(unit);
        if (unit != null)
        {
            unit.Display.ToggleSelectHighlight(true);
            SelectNode(null);
        }
    }

    public void BuildUnit(Unit unit)
    {
        SelectedNode.Builder.BuildUnit(unit);
    }

    public void PrimeAbility(Ability ability)
    {
        SelectUnit(null);
        SelectNode(null);
        primedAbility = ability;
        onPrimeAbility?.Invoke(ability);
    }

    public void LaunchAbility(Ability ability, MapNode target)
    {
        ability.Launch(target);
        PrimeAbility(null);
    }

}