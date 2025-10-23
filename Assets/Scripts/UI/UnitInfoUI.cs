using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] Image flagImage, unitIcon;
    [SerializeField] TMP_Text unitNameText;
    [SerializeField] ProgressBar unitHealthBar;
    [SerializeField] TMP_Text unitHealthText;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

    }

    private void Update()
    {

        var unit = PlayerControl.Instance.SelectedUnit;

        canvas.enabled = unit != null;
        if (unit != null)
        {
            flagImage.sprite = unit.Owner.Flag;
            unitIcon.sprite = unit.Icon;
            unitIcon.color = unit.Owner.FactionColor;
            unitNameText.text = unit.Name;
            unitHealthBar.SetLevel(unit.Health / unit.MaxHealth);
            unitHealthText.text = $"{(int)unit.Health}/{(int)unit.MaxHealth}";
        }
    }
}
