using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour, IPointerDownHandler
{

    Unit unit;

    [SerializeField] Image icon;
    [SerializeField] Image selector;
    [SerializeField] ProgressBar healthBar;
    [SerializeField] Image moveIndicator;

    public void AttachTo(Unit unit)
    {
        this.unit = unit;
        icon.color = unit.Owner.FactionColor;
        GameTick.onTick += UpdateHealth;
    }

    private void OnDestroy()
    {
        GameTick.onTick -= UpdateHealth;
    }

    private void UpdateHealth()
    {
        if (unit.Health == unit.MaxHealth)
        {
            healthBar.SetVisible(false);
        }
        else
        {
            healthBar.SetVisible(true);
            healthBar.SetLevel(unit.Health / unit.MaxHealth);
        }
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

    public void DisplayMove(MapNode target, int ticks, int maxTicks)
    {
        // moveProgressText.text = $"Moving to {target.Name}: {ticks}";
        Vector2 moveDirection = target.transform.position - unit.transform.position;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        moveIndicator.transform.parent.eulerAngles = new Vector3(0, 0, angle);
        moveIndicator.enabled = true;
        moveIndicator.fillAmount = (maxTicks - ticks) / (float)maxTicks;
    }


    public void StopMove()
    {
        moveIndicator.enabled = false;
    }
}
