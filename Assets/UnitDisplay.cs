using TMPro;
using UnityEngine;

public class UnitDisplay : MonoBehaviour
{

    Unit unit;

    [SerializeField] SpriteRenderer icon;
    [SerializeField] TMP_Text healthText;

    public void AttachTo(Unit unit)
    {
        this.unit = unit;
        icon.color = unit.Owner.FactionColor;
        GameTick.onTick += UpdateHealth;
    }

    void UpdateHealth()
    {
        healthText.text = $"{unit.Health} / {unit.MaxHealth}";
    }

}
