using UnityEngine;

public class Unit : MonoBehaviour
{
    public Faction Owner;
    public MapNode CurrentNode;

    public float Damage;    
    public float Health;
    public float MaxHealth;

    private void Awake()
    {
        
    }

    public void Place(MapNode node)
    {
        CurrentNode = node;
        node.ContainedUnit = this;
        Health = MaxHealth;
        GameTick.onTick += TickDamage;
        GetComponentInChildren<UnitDisplay>().AttachTo(this);
    }

    public void Move(MapNode node)
    {
        CurrentNode.ContainedUnit = null;
        CurrentNode = node;
        node.ContainedUnit = this;
    }

    public void TickDamage()
    {
        foreach (MapNode neighbor in CurrentNode.Neighbors)
        {
            if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.Owner != Owner)
            {
                neighbor.ContainedUnit.TakeDamage(Damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Destroy(gameObject);
        }
    }
}