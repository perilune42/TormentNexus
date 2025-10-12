using UnityEngine;

// Manages spawning and moving units globally
public class UnitController : MonoBehaviour
{
    public static UnitController instance;

    private void Awake()
    {
        instance = this;
    }

    public void MoveUnit(Unit unit, MapNode destination)
    {
        if (destination.ContainedUnit != null || destination.IsAdjacent(unit.CurrentNode))
        {
            Debug.LogError("Invalid move");
        }
        unit.Move(destination);
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