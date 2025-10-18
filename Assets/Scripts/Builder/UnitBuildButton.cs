using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitBuildButton : MonoBehaviour
{
    Unit unitTemplate;
    Button button;
    [SerializeField] TMP_Text nameText;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (PlayerControl.Instance.SelectedNode != null)
        {
            button.interactable = PlayerControl.Instance.SelectedNode.Builder.CanBuild(unitTemplate);
        }
    }

    public void SetUnit(Unit unit)
    {
        unitTemplate = unit;
        nameText.text = unit.gameObject.name;
    }

    public void Build()
    {
        PlayerControl.Instance.BuildUnit(unitTemplate);
    }
}
