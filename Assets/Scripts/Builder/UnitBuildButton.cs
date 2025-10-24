using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitBuildButton : MonoBehaviour, IPointerDownHandler
{
    Unit unitTemplate;
    Button button;
    [SerializeField] Image icon;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text infoText;

    [SerializeField] GameObject overlay;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
    }

    private void Update()
    {
        if (PlayerControl.Instance.SelectedNode != null)
        {
            SetEnabled(PlayerControl.Instance.SelectedNode.Builder.CanBuild(unitTemplate));
        }
    }

    public void SetUnit(Unit unit)
    {
        unitTemplate = unit;
        nameText.text = unit.Name;
        icon.sprite = unit.Icon;
        icon.color = FactionManager.instance.playerFaction.FactionColor;
        costText.text = unit.Cost.ToString();
        infoText.text = UnitInfoStrings.Infos[unit.Type].Desc;
        infoText.color = UnitInfoStrings.Infos[unit.Type].Color;
    }

    public void SetEnabled(bool enabled)
    {
        button.interactable = enabled;
        overlay.SetActive(!enabled);
    }

    public void Build()
    {
        AudioManager.instance.Play(AudioManager.instance.CoinsSFX);
        PlayerControl.Instance.BuildUnit(unitTemplate);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable == false)
        {
            AudioManager.instance.Play(AudioManager.instance.UIDecline);
        }
    }
}
