using UnityEngine;

public class InitialUnitPlacer : MonoBehaviour
{
    private void Start()
    {
        var foundUnits = GetComponentsInChildren<Unit>();
        foreach (var unit in foundUnits)
        {
            var node = unit.transform.parent.GetComponent<MapNode>();
            UnitController.Instance.SpawnUnit(unit, node, node.Owner);
        }
    }
}
