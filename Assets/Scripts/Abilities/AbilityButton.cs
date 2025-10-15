using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    Ability ability;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text chargeText;
    [SerializeField] TMP_Text cooldownText;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        nameText.text = ability.gameObject.name;
        GameTick.onTick += () =>
        {
            chargeText.text = $"Charges: {ability.CurrentCharges}";
            cooldownText.text = ability.CurrentCooldown.ToString();
            button.interactable = ability.CanLaunch();
        };
    }

    public void Launch()
    {
        PlayerControl.Instance.PrimeAbility(ability);
    }
}
