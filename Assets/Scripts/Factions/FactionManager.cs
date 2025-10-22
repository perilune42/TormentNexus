using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : MonoBehaviour
{
    [HideInInspector] public List<Faction> Factions;
    [HideInInspector] public List<Faction> RivalFactions;
    [HideInInspector] public List<Faction> MinorFactions;

    public static FactionManager instance;
    [HideInInspector] public Faction playerFaction;

    [SerializeField] ResourceManager resourceManagerTemplate;

    private void Awake()
    {
        instance = this;

        foreach (var faction in GetComponentsInChildren<Faction>())
        {
            Factions.Add(faction);
            if (faction.isPlayer)
            {
                playerFaction = faction;
            }
            else if (faction.isMajorFaction)
            {
                RivalFactions.Add(faction);
            }
            else
            {
                MinorFactions.Add(faction);
            }
        }

        InitFactions();
        

    }

    public void RemoveMajorStatus(Faction faction)
    {
        faction.isMajorFaction = false;
        RivalFactions.Remove(faction);
    }

    private void InitFactions()
    {
        foreach (Faction faction in Factions)
        {
            ResourceManager rm = Instantiate(resourceManagerTemplate, faction.transform);
            faction.Resource = rm;
            rm.SetFaction(faction);
        }
    }
}
