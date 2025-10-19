using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Faction owner;
    public int CurrentCharges;
    public int MaxCharges = 1;
    public int CurrentCooldown = 0;
    public int Cooldown = 100;

    public int Damage = 50;
    public int GarrisonDamage = 50;
    public int InfrastructureDamage = 50;

    [SerializeField] AbilityVFX effect;
    
    public void GiveToFaction(Faction faction)
    {
        owner = faction;
        GameTick.onTick += TickCooldown;
    }
    public virtual void Launch(MapNode target)
    {
        CurrentCharges--;
        Instantiate(effect, target.transform).Play();
    }

    public bool CanLaunch()
    {
        return CurrentCharges > 0;
    }


    public void TickCooldown()
    {
        if (CurrentCharges < MaxCharges)
        {
            CurrentCooldown--;
            if (CurrentCooldown < 0)
            {
                CurrentCharges++;
                CurrentCooldown = Cooldown;
            }
        }
        
    }
}
