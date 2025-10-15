using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    [HideInInspector] public List<Faction> factions;

    public List<SuperFaction> superFactions => factions.Where((Faction f) => f is SuperFaction).Cast<SuperFaction>().ToList();

    public static FactionManager instance;
    [HideInInspector] public SuperFaction playerFaction;

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
        foreach (SuperFaction sf in superFactions)
        {
            ResourceManager rm = Instantiate(resourceManagerTemplate, sf.transform);
            sf.Resource = rm;
            rm.Faction = sf;
        }
    }
}
