using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] Image flagImage, unitIcon;
    [SerializeField] TMP_Text factionNameText, unitNameText;
    [SerializeField] ProgressBar unitHealthBar;
    [SerializeField] TMP_Text unitHealthText, descText;

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
            factionNameText.text = unit.Owner.FactionName;
            factionNameText.color = Color.Lerp(unit.Owner.FactionColor, Color.white, 0.4f);
            unitIcon.sprite = unit.Icon;
            unitIcon.color = unit.Owner.FactionColor;
            unitNameText.text = unit.Name;
            unitHealthBar.SetLevel(unit.Health / unit.MaxHealth);
            unitHealthText.text = $"{(int)unit.Health}/{(int)unit.MaxHealth}";
            descText.text = UnitInfoStrings.Infos[unit.Type].Desc;
            descText.color = UnitInfoStrings.Infos[unit.Type].Color;
        }
    }
}
