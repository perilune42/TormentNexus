using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    [HideInInspector] public List<Faction> factions;
    public static FactionManager instance;
    [HideInInspector] public Faction playerFaction;

    [SerializeField] ResourceManager resourceManagerTemplate;

    private void Awake()
    {
        instance = this;

        foreach (var faction in GetComponentsInChildren<Faction>())
        {
            factions.Add(faction);
            if (faction is SuperFaction p && p.isPlayer)
            {
                playerFaction = p;
            }
        }

        InitFactions();

    }

    private void InitFactions()
    {
        foreach (var faction in factions)
        {
            ResourceManager rm = Instantiate(resourceManagerTemplate, faction.transform);
            faction.Resource = rm;
            rm.Faction = faction;
        }
    }
}
