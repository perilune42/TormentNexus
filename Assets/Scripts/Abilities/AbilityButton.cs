using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour, IPointerDownHandler
{
    Ability ability;
    [SerializeField] Image icon;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text chargeText;
    [SerializeField] TMP_Text cooldownText;
    [SerializeField] TMP_Text costText;

    [SerializeField] Button abilityButton, buildButton;
    [SerializeField] ProgressBar buildProgressBar;

    private void Awake()
    {
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        nameText.text = ability.Name;
        costText.text = $"{ability.Cost}";
        icon.sprite = ability.icon;

    }

    private void Update()
    {
        chargeText.text = $"{ability.CurrentCharges}/{ability.MaxCharges}";
        //cooldownText.text = ability.CurrentCooldown.ToString();
        buildProgressBar.SetLevel(Mathf.Max(0, 1 - (float)ability.CurrentCooldown / ability.BuildTime));
        buildProgressBar.SetVisible(ability.IsBuilding);
        abilityButton.interactable = ability.CanLaunch();
        buildButton.interactable = ability.CanBuild();
        icon.color = ability.CanLaunch() ? Color.white : Color.gray;


    }

    public void Build()
    {
        AudioManager.instance.Play(AudioManager.instance.CoinsSFX);
        ability.BuildNew();
    }

    public void Launch()
    {
        AudioManager.instance.Play(AudioManager.instance.softClick);
        PlayerControl.Instance.PrimeAbility(ability);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buildButton.interactable == false)
        {
            AudioManager.instance.Play(AudioManager.instance.UIDecline);
        }
    }
}
