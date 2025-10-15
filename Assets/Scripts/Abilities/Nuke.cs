public class Nuke : Ability
{
    public int Damage = 50;
    public override void Launch(MapNode target)
    {
        base.Launch(target);
        if (target.ContainedUnit != null)
        {
            target.ContainedUnit.TakeDamage(Damage);
        }
        
    }
}