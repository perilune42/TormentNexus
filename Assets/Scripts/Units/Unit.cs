using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour
{
    public Faction Owner;
    public MapNode CurrentNode;
    public UnitDisplay Display;

    public float Damage = 0.1f; // damage per tick   
    public float GarrisonDamage = 0.1f;
    public float InfrastructureDamage = 0.1f;

    public int BuildTime = 100;

    public float Health = 100f;
    public float MaxHealth = 100f;
    public float Cost = 5f;
    public float Speed = 1.0f; // speed 1.0 = 100 ticks to move 1 node

    private const int baseMoveTicks = 100;

    public MapNode MoveOrder;
    public int MoveTicksRemaining = -1;

    List<Action> onTickActions;


    private void Awake()
    {
        onTickActions = new() { TickDamage, TickMove };
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
    }

    public void Move(MapNode node)
    {
        CurrentNode.ContainedUnit = null;
        CurrentNode = node;
        node.ContainedUnit = this;
        transform.SetParent(node.transform, false);
    }

    public void TickDamage()
    {
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
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            TriggerDeath();
        }
    }

    public void TriggerDeath()
    {
        foreach (Action action in onTickActions)
        {
            GameTick.onTick -= action;
        }
        UnitController.Instance.RemoveUnit(this);
    }

    public void StartMove(MapNode target)
    {
        MoveOrder = target;
        MoveTicksRemaining = (int)(baseMoveTicks / Speed);
        Display.DisplayMove(target, MoveTicksRemaining);
    }

    private void TickMove()
    {
        if (MoveOrder == null) return;
        Display.DisplayMove(MoveOrder, MoveTicksRemaining);
        if (MoveTicksRemaining <= 0)
        {
            if (UnitController.Instance.IsValidMove(this, MoveOrder) && (MoveOrder.Owner == Owner || MoveOrder.IsCapturable()))
            {
                FinishMove();
            }
        }
        else
        {
            MoveTicksRemaining--;
        }
        
    }

    public void CancelMove()
    {
        MoveOrder = null;
        MoveTicksRemaining = -1;
        Display.StopMove();
    }

    public void FinishMove()
    {
        Move(MoveOrder);
        if (MoveOrder.Owner != Owner)
        {
            MoveOrder.Capture(Owner);
        }
        CancelMove();
    }

}