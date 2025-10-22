using NUnit.Framework;
using TMPro;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public static BuildMenu Instance;

    [SerializeField] UnitBuildButton buttonTemplate;
    [SerializeField] TMP_Text buildProgressText;

    

    private void Awake()
    {
        Instance = this;
        /*
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
        */

        
    }

    public void UpdateBuildables()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (PlayerControl.Instance.SelectedNode == null ||
            PlayerControl.Instance.SelectedNode.Builder.Enabled == false)
        {
            return;
        }
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
