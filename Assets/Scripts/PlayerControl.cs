using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;
    public MapNode HoveredNode;
    public MapNode SelectedNode;
    public Unit SelectedUnit;
    public Action<MapNode> onSelectNode;
    public Action<Unit> onSelectUnit;
    private void Awake()
    {
        Instance = this;
    }

    public void SelectNode(MapNode node)
    {
        if (SelectedNode != null) SelectedNode.ToggleSelectHighlight(false);
        SelectedNode = node;
        onSelectNode?.Invoke(node);
        if (node != null) node.ToggleSelectHighlight(true);
    }


    public void HoverNode(MapNode node)
    {
        HoveredNode = node;
    }

    public void RClickNode(MapNode node)
    {
        if (SelectedUnit != null && UnitController.Instance.IsValidMove(SelectedUnit,node)) 
        {
            UnitController.Instance.MoveUnit(SelectedUnit, node);
        }
    }

    public void SelectUnit(Unit unit)
    {
        if (SelectedUnit != null) SelectedUnit.Display.ToggleSelectHighlight(false);
        SelectedUnit = unit;
        onSelectUnit?.Invoke(unit);
        if (unit != null) unit.Display.ToggleSelectHighlight(true);
    }

    public void BuildUnit(Unit unit)
    {
        Builder.BuildUnit(FactionManager.instance.playerFaction, SelectedNode, unit);
    }


}