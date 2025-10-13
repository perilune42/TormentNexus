using TMPro;
using UnityEngine;

public class UnitInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TMP_Text unitNameText;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        PlayerControl.Instance.onSelectUnit += DisplayForUnit;
        
    }

    private void DisplayForUnit(Unit unit)
    {
        canvas.enabled = unit != null;
        if (unit != null)
        {
            unitNameText.text = unit.gameObject.name;
        }
    }
}
