using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public enum NodeType
{
    Capital, City, Military, Science
}

public class MapNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Info
    public string Name;
    public Faction Owner;

    public NodeType Type;

    // Editor Refs
    public List<MapNode> Neighbors;
    public TMP_Text NameTextMeshPro;

    // Self Refs
    private LineRenderer lineRenderer;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer overlay;
    public SpriteRenderer selector;
    public Light2D colorLight;

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

    public static MapNode DummyNode;
    public bool IsDummyNode = false;

    [SerializeField] ProgressBar garrisonHPBar;
    [SerializeField] ProgressBar infrastructureHPBar;

    [SerializeField] TMP_Text nodeTypeText;

    public Action<Unit> onUnitEnter;

    private void Awake()
    {
        if (IsDummyNode) 
        { 
            DummyNode = this;
            return;
        }
        AssignOwnRefs();
        Owner.AllNodes.Add(this);
        Builder.SetNode(this);

        GameTick.onTick += Tick;

        if (Type == NodeType.Capital)
        {
            if (Owner.Capital != null)
            {
                Debug.LogError($"Duplicate Capital: {Name}");
            }
            Owner.Capital = this;
        }

        if (Type != NodeType.Military && Type != NodeType.Capital)
        {
            Builder.Enabled = false;
        }
        if (Type == NodeType.Capital || Type == NodeType.Military)
        {
            MaxGarrisonHealth *= 1.5f;
            GarrisonHealth *= 1.5f;
        }
        if (Type == NodeType.City || Type == NodeType.Capital)
        {
            MaxInfrastructureHealth *= 2f;
            InfrastructureHealth *= 2f;
        }



    }

    private void OnDestroy()
    {
        GameTick.onTick -= Tick;
    }
    private void Tick()
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

        Map map = FindFirstObjectByType<Map>();
        switch (Type)
        {
            case NodeType.Capital:
                spriteRenderer.sprite = map.CapitalIcon; break;
            case NodeType.City:
                spriteRenderer.sprite = map.CityIcon; break;
            case NodeType.Military:
                spriteRenderer.sprite = map.MilitaryIcon; break;
            case NodeType.Science:
                spriteRenderer.sprite = map.ScienceIcon; break;
        }
        overlay.sprite = spriteRenderer.sprite;
        overlay.gameObject.SetActive(false);
            


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
        if (ContainedUnit != null && ContainedUnit.Display.hovered)
        {
            return;
        }
        PlayerControl.Instance.HoverNode(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.Play(AudioManager.instance.MapNodeClick);
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
        if (Type == NodeType.City) nodeTypeText.text = "C";
        else if (Type == NodeType.Capital) nodeTypeText.text = "*";
        else if (Type == NodeType.Military) nodeTypeText.text = "M";
        else nodeTypeText.text = "S";
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
        if (InfrastructureHealth <= 0f)
        {
            damage *= 3f;
        }
        else if (InfrastructureHealth <= 0.5f * MaxInfrastructureHealth)
        {
            damage *= 2f;
        }
        GarrisonHealth -= damage;
        healTimer = healCooldown;
        if (GarrisonHealth < 0) GarrisonHealth = 0;
    }

    public void TakeInfrastructureDamage(float damage)
    {
        PopulationCounter.Instance.DealDamage(damage);
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

        if (Type == NodeType.Capital)
        {
            if (oldOwner == FactionManager.instance.playerFaction)
            {
                GameManager.instance.FactionVictory(newOwner);
            }
            else
            {
                FactionManager.instance.RemoveMajorStatus(oldOwner);
                
                if (FactionManager.instance.RivalFactions.Count <= 0)
                {
                    GameManager.instance.FactionVictory(FactionManager.instance.playerFaction);
                }
            }

            Type = NodeType.City;
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
