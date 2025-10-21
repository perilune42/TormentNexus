using UnityEngine;

public class HateMeter : MonoBehaviour
{
    public Faction Faction;
    public float CurrentHate = 0;
    public float TempHate = 0;
    public const float MaxHate = 100f;
    private const float decayRate = 0.01f;
    private const float tempHateDecayRate = 0.1f;

    public const float HatePerCapture = 3f;

    private const float tempHateMultiplier = 3f;

    public void SetToFaction(Faction faction)
    {
        this.Faction = faction;
        GameTick.onTick += HateDecay;
        GameTick.onTick += TempHateDecay;
    }

    public void AddHate(float amount, bool fromWeaponUse = false)
    {
        if (fromWeaponUse)
        {
            CurrentHate += amount * tempHateMultiplier;
            TempHate += amount * (tempHateMultiplier - 1);
            if (Faction.AIControl != null) Faction.AIControl.TempAbilityBoost();
            
        }
        else
        {
            CurrentHate += amount;
        }
    }

    private void HateDecay()
    {
        CurrentHate -= decayRate;
        if (CurrentHate < 0) CurrentHate = 0;
    }

    private void TempHateDecay()
    {
        if (TempHate > 0)
        {
            CurrentHate -= tempHateDecayRate;
            TempHate -= tempHateDecayRate;
            if (CurrentHate < 0) CurrentHate = 0;
        }
    }

}
