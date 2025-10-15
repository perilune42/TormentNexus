using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    Faction owner;
    public int CurrentCharges;
    public int MaxCharges = 3;
    public int CurrentCooldown = 0;
    public int Cooldown = 100;
    
    public void GiveToFaction(Faction faction)
    {
        owner = faction;
        GameTick.onTick += TickCooldown;
    }
    public virtual void Launch(MapNode target)
    {
        CurrentCharges--;
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
