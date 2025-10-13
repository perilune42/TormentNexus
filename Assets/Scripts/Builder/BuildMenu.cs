using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    
    [SerializeField] UnitBuildButton buttonTemplate;
    private void Awake()
    {
        foreach (Unit unit in FactionManager.instance.playerFaction.BuildableUnits)
        {
            CreateButtonForUnit(unit);
        }
        
    }

    private void CreateButtonForUnit(Unit unit)
    {
        var newButton = Instantiate(buttonTemplate, transform);
        newButton.SetUnit(unit);
    }


}
