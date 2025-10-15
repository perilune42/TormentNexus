public class Nuke : Ability
{
    public int Damage = 50;
    public override void Launch(MapNode target)
    {
        if (target.ContainedUnit != null)
        {
            target.ContainedUnit.TakeDamage(50);
        }
    }
}