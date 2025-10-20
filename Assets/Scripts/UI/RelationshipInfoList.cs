using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipInfoList : MonoBehaviour
{
    
    public static RelationshipInfoList Instance;
    [SerializeField] RelationshipInfo infoTemplate;

    private void Start()
    {
        SetFactions(FactionManager.instance.RivalFactions);
    }

    public void SetFactions(List<Faction> factions)
    {
        foreach (Faction faction in factions)
        {
            var newInfo = Instantiate(infoTemplate, transform);
            newInfo.SetFaction(faction);
        }
    }
}
