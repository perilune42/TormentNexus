public class Nuke : Ability
{
    public override void Launch(MapNode target)
    {
        base.Launch(target);
        if (target.ContainedUnit != null)
        {
            target.ContainedUnit.TakeDamage(Damage);
        }

        target.TakeGarrisonDamage(GarrisonDamage);
        target.TakeInfrastructureDamage(InfrastructureDamage);
        
    }
}