using Mono.Cecil;
using System.Security.Cryptography;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Faction Faction;


    public float ResearchPoints;

    public float ResourceAmount = 0;

    private void Awake()
    {
        GameTick.onDay += GainResearchPoint;
        GameTick.onDay += GenerateResource;
    }
    private void GenerateResource()
    {
        ResourceAmount += 0f;
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
