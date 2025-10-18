using NUnit.Framework;
using TMPro;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    
    [SerializeField] UnitBuildButton buttonTemplate;
    [SerializeField] TMP_Text buildProgressText;

    

    private void Awake()
    {
        foreach (Unit unit in FactionManager.instance.playerFaction.BuildableUnits) 
        {
            CreateButtonForUnit(unit);
        }
        GameTick.onTick += () =>
        {
            if (PlayerControl.Instance.SelectedNode != null)
            {
                MapNode node = PlayerControl.Instance.SelectedNode;
                if (node.Builder.PendingBuild != null)
                {
                    buildProgressText.text = $"Building {node.Builder.PendingBuild}: {node.Builder.TimeRemaining}";
                }
                else
                {
                    buildProgressText.text = null;
                }
            }
            else
            {
                buildProgressText.text = null;
            }
        };

        
    }

    private void CreateButtonForUnit(Unit unit)
    {
        var newButton = Instantiate(buttonTemplate, transform);
        newButton.SetUnit(unit);
    }


}
