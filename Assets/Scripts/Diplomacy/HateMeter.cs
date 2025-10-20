using UnityEngine;

public class HateMeter : MonoBehaviour
{
    public Faction Faction;
    public float CurrentHate = 0;
    public const float MaxHate = 100f;
    private const float decayRate = 0.01f;

    public const float HatePerCapture = 3f;

    public void SetToFaction(Faction faction)
    {
        this.Faction = faction;
        GameTick.onTick += HateDecay;
    }

    public void AddHate(float amount)
    {
        CurrentHate = Mathf.Clamp(CurrentHate + amount, 0, MaxHate);
    }

    private void HateDecay()
    {
        CurrentHate = Mathf.Clamp(CurrentHate - decayRate, 0, MaxHate);
    }
}
