using TMPro;
using UnityEngine;

public class UnitBuildButton : MonoBehaviour
{
    Unit unitTemplate;
    [SerializeField] TMP_Text nameText;

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
