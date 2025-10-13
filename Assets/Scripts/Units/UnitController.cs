using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Manages spawning and moving units globally
public class UnitController : MonoBehaviour
{
    public static UnitController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void MoveUnit(Unit unit, MapNode destination)
    {
        //if (!IsValidMove(unit,destination))
        //{
        //    Debug.LogError("Invalid move");
        //})
        if (unit.MoveOrder == unit.CurrentNode)
        {
            unit.CancelMove();
        }
        else
        {
            unit.StartMove(destination);
        }
    }

    public bool IsValidMove(Unit unit, MapNode destination, bool ignoreOccupied = false)
    {
        return (destination.ContainedUnit == null || ignoreOccupied) && destination.IsAdjacent(unit.CurrentNode);
    }

    public void SpawnUnit(Unit unit, MapNode destination, Faction owner) 
    {
        if (destination.ContainedUnit != null)
        {
            Debug.LogError("Invalid spawn");
        }
        unit.Owner = owner;
        unit.Place(destination);
    }
}