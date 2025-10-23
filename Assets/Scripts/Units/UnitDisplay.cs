using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDisplay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    Unit unit;

    public bool hovered = false;

    [SerializeField] Image icon;
    [SerializeField] Image overlay;
    [SerializeField] Image selector;
    [SerializeField] ProgressBar healthBar;
    [SerializeField] Image moveIndicator;

    

private void Awake()
    {
        overlay.gameObject.SetActive(false);
    }

    public void AttachTo(Unit unit)
    {
        this.unit = unit;
        icon.color = unit.Owner.FactionColor;
        icon.sprite = unit.Icon;
        overlay.sprite = icon.sprite;
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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerControl.Instance.RClickNode(unit.CurrentNode);
        }
    }

    public void DisplayMove(MapNode target, int ticks, int maxTicks)
    {
        // moveProgressText.text = $"Moving to {target.Name}: {ticks}";
        Vector2 moveDirection = target.transform.position - unit.transform.position;
        float moveDistance = (target.transform.position - unit.transform.position).magnitude / 2.0f;
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        moveIndicator.transform.parent.eulerAngles = new Vector3(0, 0, angle);
        moveIndicator.enabled = true;
        moveIndicator.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(0.25f, moveDistance, (maxTicks - ticks) / (float)maxTicks), moveIndicator.rectTransform.sizeDelta.y);
    }


    public void StopMove()
    {
        moveIndicator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overlay.gameObject.SetActive(true);
        hovered = true;
        MapNode node = GetComponentInParent<MapNode>();
        node.overlay.gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overlay.gameObject.SetActive(false);
        hovered = false;
        MapNode node = GetComponentInParent<MapNode>();
        if (PlayerControl.Instance.HoveredNode != null)
        {
            node.overlay.gameObject.SetActive(true);
        }
    }
}
