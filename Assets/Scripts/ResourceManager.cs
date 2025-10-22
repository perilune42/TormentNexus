using Mono.Cecil;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Faction Faction;


    public float ResearchPoints;

    public float ResourceAmount = 0;

    

    private const float ResourcePerMilitaryNode = 0.2f;
    private const float ResourcePerCapital = 1f;

    public float ResourceGeneration()
    {
        float gen = 0;
        if (Faction.AllNodes.Where((node) => node.Type == NodeType.Capital).Count() > 0)
        {
            gen += ResourcePerCapital;
        }
        gen += Faction.AllNodes.Where((node) => node.Type == NodeType.Military).Count() * ResourcePerMilitaryNode;
        return gen;
    } 

    private void Awake()
    {
        GameTick.onDay += GainResearchPoint;
        GameTick.onDay += GenerateResource;
    }
    private void GenerateResource()
    {
        ResourceAmount = Mathf.Round((ResourceGeneration() + ResourceAmount) * 100f) / 100f;
    }

    public void ConsumeResource(float amount)
    {
        ResourceAmount -= amount;
    }

    private void GainResearchPoint()
    {
        ResearchPoints += 1.5f;
        // scuffed, per-faction research later
        if (Faction == FactionManager.instance.playerFaction)
        {
            TechTree.instance.AddResearchPoints(ResearchPoints);
            ResearchPoints = 0;
        }
    }



}
