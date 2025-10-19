using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    Ability ability;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text chargeText;
    [SerializeField] TMP_Text cooldownText;

    [SerializeField] Button abilityButton, buildButton;
    [SerializeField] ProgressBar buildProgressBar;

    private void Awake()
    {
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        nameText.text = ability.gameObject.name;

    }

    private void Update()
    {
        chargeText.text = $"Charges: {ability.CurrentCharges}";
        //cooldownText.text = ability.CurrentCooldown.ToString();
        buildProgressBar.SetLevel(Mathf.Max(0, 1 - (float)ability.CurrentCooldown / ability.BuildTime));
        buildProgressBar.SetVisible(ability.IsBuilding);
        abilityButton.interactable = ability.CanLaunch();
        buildButton.interactable = ability.CanBuild();
    }

    public void Build()
    {
        ability.BuildNew();
    }

    public void Launch()
    {
        PlayerControl.Instance.PrimeAbility(ability);
    }
}
