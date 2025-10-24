using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public enum UnitType
{
    Infantry, Armor, Airforce, Artillery, Terror
}
public class Unit : MonoBehaviour
{
    public Faction Owner;
    public MapNode CurrentNode;
    public UnitDisplay Display;

    public string Name;
    public Sprite Icon;
    public UnitType Type;

    public float Damage = 0.1f; // damage per tick   
    public float GarrisonDamage = 0.1f;
    public float InfrastructureDamage = 0.1f;

    public int BuildTime = 100;

    public float Health = 100f;
    public float MaxHealth = 100f;
    public float Cost = 5f;
    public float Speed = 1.0f; // speed 1.0 = 100 ticks to move 1 node

    private const int baseMoveTicks = 100;
    private int moveTicks => (int)(baseMoveTicks / Speed);

    public MapNode MoveOrder;
    public MapNode Waypoint;

    [HideInInspector] public int MoveTicksRemaining = -1;

    List<Action> onTickActions;

    [HideInInspector] public MapNode AttackingNode;
    [HideInInspector] public List<MapNode> DefendingAgainstNodes;

    private const int healCooldown = 150;
    private int healTimer = 0;

    public float healSpeed = 0.03f;

    public Action<MapNode> OnMoved; // arg: node this unit came from; to access where the unit moved to just use CurrentNode

    private void Awake()
    {
        onTickActions = new() { TickDamage, TickMove, Heal };
    }

    public void Place(MapNode node)
    {
        CurrentNode = node;
        node.ContainedUnit = this;
        transform.SetParent(node.transform, false);
        Health = MaxHealth;
        foreach (Action action in onTickActions)
        {
            GameTick.onTick += action;
        }
        Display = GetComponentInChildren<UnitDisplay>();
        Display.AttachTo(this);
        node.onUnitEnter?.Invoke(this);

        foreach (MapNode neighbor in CurrentNode.Neighbors)
        {
            if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.AttackingNode == CurrentNode)
            {
                OnAttackedBy(neighbor);
            }
        }
    }

    public void Move(MapNode node)
    {
        DefendingAgainstNodes.Clear();
        StopAttacking();

        var prevNode = CurrentNode;

        CurrentNode.ContainedUnit = null;
        CurrentNode = node;
        node.ContainedUnit = this;
        transform.SetParent(node.transform, false);
        CurrentNode.onUnitEnter?.Invoke(null);
        node.onUnitEnter?.Invoke(this);

        foreach (MapNode neighbor in CurrentNode.Neighbors)
        {
            if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.AttackingNode == CurrentNode)
            {
                if (neighbor.ContainedUnit.Owner == Owner)
                {
                    // should only happen on capture
                    neighbor.ContainedUnit.StopAttacking();
                }
                else
                {
                    OnAttackedBy(neighbor);
                }
                    
            }
        }

        OnMoved?.Invoke(prevNode);


    }

    public void TickDamage()
    {
        /* OLD ATTACK CODE
         
        foreach (MapNode neighbor in CurrentNode.Neighbors)
        {
            if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.Owner != Owner)
            {
                neighbor.ContainedUnit.TakeDamage(Damage);
            }
            if (neighbor.Owner != Owner)
            {
                neighbor.TakeGarrisonDamage(GarrisonDamage);
                if (neighbor.GarrisonHealth > 0)
                {
                    neighbor.TakeInfrastructureDamage(InfrastructureDamage);
                }
            }
        }
        */

        if (AttackingNode == null)
        {
            // Defending mode, split damage
            int attackerCount = DefendingAgainstNodes.Count;
            if (attackerCount == 0) return;

            for (int i = attackerCount - 1; i >= 0; i--)
            {
                MapNode attacker = DefendingAgainstNodes[i];
                if (attacker.ContainedUnit == null)
                {
                    Debug.LogError("Being attacked by empty node");
                    DefendingAgainstNodes.Remove(attacker);
                }
                DamageNode(attacker, attackerCount);
            }
        }
        else
        {
            // Attacking mode
            DamageNode(AttackingNode, 1);
        }
    }

    private void DamageNode(MapNode node, int split = 1)
    {
        float randFactor = Random.Range(0.7f, 1.3f);
        healTimer = healCooldown;
        if (node.ContainedUnit != null)
        {
            node.ContainedUnit.TakeDamage((Damage / split) * randFactor );
        }

        node.TakeGarrisonDamage((GarrisonDamage / split) * randFactor);
        if (node.GarrisonHealth > 0)
        {
            node.TakeInfrastructureDamage((InfrastructureDamage / split) * randFactor);
        }
    }

    public void TakeDamage(float damage)
    {
        if (CurrentNode.InfrastructureHealth <= 0f)
        {
            damage *= 1.5f;
        }
        Health -= damage;
        healTimer = healCooldown;
        if (Health <= 0)
        {
            TriggerDeath();
        }
    }

    private void Heal()
    {
        if (healTimer == 0)
        {
            Health += healSpeed;
            if (Health > MaxHealth) Health = MaxHealth;
        }
        else
        {
            healTimer--;
        }
    }

    public void TriggerDeath()
    {
        if (AttackingNode != null) StopAttacking();
        foreach (Action action in onTickActions)
        {
            GameTick.onTick -= action;
        }
        UnitController.Instance.RemoveUnit(this);
    }

    public void StartMove(MapNode target)
    {
        if (target == MoveOrder) return;
        StopAttacking();
        MoveOrder = target;
        MoveTicksRemaining = moveTicks;
        Display.DisplayMove(target, MoveTicksRemaining, moveTicks);
    }

    public void StartPath(MapNode target)
    {
        Waypoint = target;
        ContinuePath();
    }

    public void CancelPath()
    {
        Waypoint = null;
    }

    private void ContinuePath()
    {
        if (Waypoint == null) return;
        if (CurrentNode == Waypoint)
        {
            Waypoint = null;
            return;
        }

        // try to avoid blocking units unless no path found
        var nextMove = Util.Pathfind(this, Waypoint, avoidFriendlyCollision:false);
        if (nextMove == null) nextMove = Util.Pathfind(this, Waypoint, avoidFriendlyCollision: false);
        if (nextMove == null)
        {
            Debug.LogError("Pathfinding failed");
            // failed to find a path
            Waypoint = null;
            return;
        }
        StartMove(nextMove);
    }

    private void TickMove()
    {
        if (MoveOrder == null) return;
        Display.DisplayMove(MoveOrder, MoveTicksRemaining, moveTicks);
        if (MoveTicksRemaining <= 0)
        {
            if (UnitController.Instance.IsValidMove(this, MoveOrder) && (MoveOrder.Owner == Owner || MoveOrder.IsCapturable()))
            {
                FinishMove();

            }
            else if (MoveOrder.ContainedUnit != null && MoveOrder.ContainedUnit.Owner == Owner 
                && MoveOrder.ContainedUnit.MoveOrder == CurrentNode && MoveOrder.ContainedUnit.MoveTicksRemaining <= 1)
            {
                MapNode originalDest = MoveOrder;
                Unit toSwap = MoveOrder.ContainedUnit;
                MoveOrder = MapNode.DummyNode;
                FinishMove(false);
                toSwap.FinishMove();
                MoveOrder = originalDest;
                FinishMove();

            }
            else if (AttackingNode == null && MoveOrder.Owner != Owner)
            {
                StartAttacking(MoveOrder);
            }
        }
        else
        {
            MoveTicksRemaining--;
        }
        
    }

    public void CancelMove()
    {
        StopAttacking();
        MoveOrder = null;
        MoveTicksRemaining = -1;
        Display.StopMove();
    }

    public void FinishMove(bool updatePathfinding = true)
    {
        var order = MoveOrder;
        CancelMove();
        if (!order.IsDummyNode && order.Owner != Owner)
        {
            order.Capture(Owner);
        }
        Move(order);
        if (updatePathfinding)
        {
            ContinuePath();
        }
    }

    public void StartAttacking(MapNode node)
    {
        StopAttacking();
        AttackingNode = node;
        if (node.ContainedUnit != null)
        {
            node.ContainedUnit.OnAttackedBy(CurrentNode);
        }
    }

    public void StopAttacking()
    {
        if (AttackingNode == null) return;
        if (AttackingNode.ContainedUnit != null)
        {
            AttackingNode.ContainedUnit.StopBeingAttackedBy(CurrentNode);
        }
        AttackingNode = null;
    }

    public void OnAttackedBy(MapNode node)
    {
        DefendingAgainstNodes.Add(node);
    }

    public void StopBeingAttackedBy(MapNode node)
    {
        DefendingAgainstNodes.Remove(node);
    }


}