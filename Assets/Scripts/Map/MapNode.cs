using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class MapNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Info
    public string Name;
    public Faction Owner;

    // Editor Refs
    public List<MapNode> Neighbors;
    public TMP_Text NameTextMeshPro;

    // Self Refs
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer selector;
    [SerializeField] private Light2D colorLight;

    public Unit ContainedUnit = null;

    public float GarrisonHealth = 50;
    public float MaxGarrisonHealth = 50;

    public float InfrastructureHealth = 100;
    public float MaxInfrastructureHealth = 100;

    public float DefenseDamage = 0.05f;

    private const int healCooldown = 200;
    private int healTimer = 0;

    private float garrisonHealSpeed = 0.03f;
    private float infrastructureHealSpeed = 0.01f;

    public Builder Builder;

    [SerializeField] ProgressBar garrisonHPBar;
    [SerializeField] ProgressBar infrastructureHPBar;

    public Action<Unit> onUnitEnter;

    private void Awake()
    {
        
        AssignOwnRefs();
        Owner.AllNodes.Add(this);
        Builder.SetNode(this);

        GameTick.onTick += () =>
        {
            if (GarrisonHealth >= MaxGarrisonHealth)
            {
                garrisonHPBar.SetVisible(false);
            }
            else
            {
                garrisonHPBar.SetVisible(true);
                garrisonHPBar.SetLevel(GarrisonHealth / MaxGarrisonHealth);
            }
            if (InfrastructureHealth >= MaxInfrastructureHealth)
            {
                infrastructureHPBar.SetVisible(false);
            }
            else
            {
                infrastructureHPBar.SetVisible(true);
                infrastructureHPBar.SetLevel(InfrastructureHealth / MaxInfrastructureHealth);
            }
            DamageAttackers();
            Heal();

        };
    }

    // TODO - Double draws, fix
    // TODO - Also draw in editor
    public void DrawNode()
    {
        AssignOwnRefs(); 

        // Name text
        NameTextMeshPro.text = Name;

        // Faction color
        if (Owner != null)
        {
            SetColor();
        } 
            


        // Lines, currently scuffed
        lineRenderer.positionCount = Neighbors.Count * 2 + 1;
        lineRenderer.SetPosition(0, transform.position);
    
        int i = 1;
        foreach (MapNode neighbor in Neighbors)
        {
            lineRenderer.SetPosition(i, neighbor.transform.position);
            lineRenderer.SetPosition(i + 1, transform.position);
            i += 2;
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerControl.Instance.HoverNode(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerControl.Instance.HoverNode(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerControl.Instance.SelectNode(this);

            // Debug
            // spriteRenderer.color = FactionManager.instance.playerFaction.FactionColor;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerControl.Instance.RClickNode(this);
        }
    }

    private void AssignOwnRefs()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool IsAdjacent(MapNode neighbor)
    {
        return Neighbors.Contains(neighbor);
    }

    public void ToggleSelectHighlight(bool toggle)
    {
        selector.enabled = toggle;
    }

    public void TakeGarrisonDamage(float damage)
    {
        if (ContainedUnit != null) damage *= 0.5f;
        if (InfrastructureHealth <= 0.5f * MaxInfrastructureHealth)
        {
            damage *= 2f;
        }
        GarrisonHealth -= damage;
        healTimer = healCooldown;
        if (GarrisonHealth < 0) GarrisonHealth = 0;
    }

    public void TakeInfrastructureDamage(float damage)
    {
        InfrastructureHealth -= damage;
        healTimer = healCooldown;
        if (InfrastructureHealth < 0) InfrastructureHealth = 0;
    }

    private void DamageAttackers()
    {
        // cannot defend if infrastructure is damaged or garrison has 0 health
        if (InfrastructureHealth <= 0.5f * MaxInfrastructureHealth || GarrisonHealth <= 0) return;

        var neighboringEnemyUnits = Util.GetNeighboringEnemyUnits(this);
        List<Unit> attackers = new();
        foreach (Unit attacker in neighboringEnemyUnits)
        {
            if (attacker.AttackingNode == this)
            {
                attackers.Add(attacker);
            }
        }
        foreach (Unit attacker in attackers)
        {
            attacker.TakeDamage(DefenseDamage / attackers.Count);
        }
    }

    private void Heal()
    {
        if (healTimer == 0)
        {
            float garrisonHealFactor = 1;
            if (InfrastructureHealth < MaxInfrastructureHealth * 0.5f) garrisonHealFactor *= 0.25f;
            GarrisonHealth += garrisonHealSpeed * garrisonHealFactor;
            if (GarrisonHealth > MaxGarrisonHealth) GarrisonHealth = MaxGarrisonHealth;
            InfrastructureHealth += infrastructureHealSpeed;
            if (InfrastructureHealth > MaxInfrastructureHealth) InfrastructureHealth = MaxInfrastructureHealth;
        }
        else
        {
            healTimer--;
        }
    }

    public void Capture(Faction newOwner)
    {
        Faction oldOwner = Owner;
        Owner.AllNodes.Remove(this);
        Owner = newOwner;
        newOwner.AllNodes.Add(this);
        SetColor();
        GarrisonHealth = 0.5f * MaxGarrisonHealth;

        if (oldOwner.HateMeter != null && newOwner.isPlayer)
        {
            oldOwner.HateMeter.AddHate(HateMeter.HatePerCapture);
        }
    }
    
    public bool IsCapturable()
    {
        return GarrisonHealth <= 0;
    }

    private void SetColor()
    {
        spriteRenderer.color = Owner.FactionColor;
        colorLight.color = Color.Lerp(Owner.FactionColor, Color.black, 0.4f);
        NameTextMeshPro.color = Color.Lerp(Owner.FactionColor, Color.white, 0.4f);
    }
}
