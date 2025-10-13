using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitDisplay : MonoBehaviour, IPointerDownHandler
{

    Unit unit;

    [SerializeField] SpriteRenderer icon;
    [SerializeField] SpriteRenderer selector;
    [SerializeField] TMP_Text healthText;

    [SerializeField] TMP_Text moveProgressText;

    public void AttachTo(Unit unit)
    {
        this.unit = unit;
        icon.color = unit.Owner.FactionColor;
        GameTick.onTick += UpdateHealth;
    }

    private void UpdateHealth()
    {
        healthText.text = $"{(int)unit.Health} / {(int)unit.MaxHealth}";
    }

    public void ToggleSelectHighlight(bool toggle)
    {
        selector.enabled = toggle;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerControl.Instance.SelectUnit(unit);
        }
    }

    public void DisplayMove(MapNode target, int ticks)
    {
        moveProgressText.text = $"Moving to {target.Name}: {ticks}";
    }

    public void StopMove()
    {
        moveProgressText.text = "";
    }
}
