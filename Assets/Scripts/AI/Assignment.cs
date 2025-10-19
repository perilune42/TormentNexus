
// high-level goal assigned to a particular unit
public abstract class Assignment
{
    public Unit unit;
    public abstract bool Interruptable { get; } // whether this assignment can interrupt ongoing moves

    public Assignment(Unit unit) {
        this.unit = unit;
    }

    public abstract MapNode GetNextMove();
}


// Find an enemy node to attack and cause general damage
public class CaptureAssignment : Assignment
{
    public CaptureAssignment(Unit unit) : base(unit) { }
    public override bool Interruptable => false;

    public override MapNode GetNextMove()
    {
        var neighboringEnemies = Util.GetNeighboringEnemies(unit.CurrentNode);
        if (neighboringEnemies.Count == 0)
        {
            MapNode target = Util.PathfindConditional(unit, (node) =>
                node.Owner == unit.Owner && Util.GetNeighboringEnemies(node).Count > 0
            );
            return target;
        }
        return neighboringEnemies.GetRandom();
    }
}

// move to a safe location to heal
public class RetreatAssignment : Assignment
{
    public RetreatAssignment(Unit unit) : base(unit) { }
    public override bool Interruptable => true;

    public override MapNode GetNextMove()
    {
        if (Util.GetNeighboringEnemies(unit.CurrentNode).Count == 0) return null;

        // find a move that leads to a safe location
        MapNode target = Util.PathfindConditional(unit, 
            (node) =>
                node.Owner == unit.Owner && Util.GetNeighboringEnemies(node).Count == 0
        );
        // if no inner node available, find a place with no nearby enemy units
        if (target == null)
        {
            target = Util.PathfindConditional(unit, 
                (node) =>
                    node.Owner == unit.Owner && Util.GetNeighboringEnemyUnits(node).Count == 0
            );
        }
        return target;
    }
}

// find a node under attack and defend it; stand guard at border otherwise
public class DefendAssignment : Assignment
{
    public DefendAssignment(Unit unit) : base(unit) { }
    public override bool Interruptable => true;

    public override MapNode GetNextMove()
    {
        // find a node that is actively under attack by an enemy
        MapNode target = Util.PathfindConditional(unit,
            (node) =>
            {
                if (node.Owner != unit.Owner) return false;
                foreach (Unit attacker in Util.GetNeighboringEnemyUnits(node))
                {
                    if (attacker.MoveOrder == node) return true;
                }
                return false;
            }, 
            (node) =>   // avoid enemy territory
                node.Owner != unit.Owner
        );
        // if not found, find a node that is on the border
        if (target == null)
        {
            target = Util.PathfindConditional(unit, (node) =>
                node.Owner == unit.Owner && Util.GetNeighboringEnemies(node).Count > 0
            );
        }
        return target;
    }
}

// Specifically seek out enemy units and attack them
public class EliminateAssignment : Assignment
{
    public EliminateAssignment(Unit unit) : base(unit) { }
    public override bool Interruptable => true;

    public override MapNode GetNextMove()
    {
        MapNode target = Util.PathfindConditional(unit, (node) =>
            node.ContainedUnit != null && node.ContainedUnit.Owner != unit.Owner
        );
        return target;
    }
}
