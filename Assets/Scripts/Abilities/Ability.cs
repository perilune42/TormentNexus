using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected Faction owner;
    public int CurrentCharges;
    public int MaxCharges = 1;
    public int CurrentCooldown = 0;
    public int BuildTime = 100;

    public int Cost;
    

    public float Damage = 50;
    public float GarrisonDamage = 50;
    public float InfrastructureDamage = 50;

    public float HateGeneration = 10f;

    public bool IsBuilding = false;
    

    [SerializeField] AbilityVFX effect;
    
    public void GiveToFaction(Faction faction)
    {
        owner = faction;
        GameTick.onTick += TickCooldown;
    }

    public void BuildNew()
    {
        if (!CanBuild()) Debug.LogError("Invalid ability build");
        owner.Resource.ResourceAmount -= Cost;
        IsBuilding = true;
        CurrentCooldown = BuildTime;
    }

    public bool CanBuild()
    {
        return !IsBuilding && owner.Resource.ResourceAmount >= Cost && CurrentCharges < MaxCharges;
    }

    public virtual void Launch(MapNode target)
    {
        CurrentCharges--;
        Instantiate(effect, target.transform).Play();
        if (target.Owner.HateMeter != null && owner.isPlayer)
        {
            target.Owner.HateMeter.AddHate(HateGeneration);
        }
    }

    public bool CanLaunch()
    {
        return CurrentCharges > 0;
    }


    public void TickCooldown()
    {
        if (!IsBuilding) return;
        if (CurrentCharges < MaxCharges)
        {
            CurrentCooldown--;
            if (CurrentCooldown < 0)
            {
                CurrentCharges++;
                IsBuilding = false;   
            }
        }
        
    }
}
