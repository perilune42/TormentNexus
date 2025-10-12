using Mono.Cecil;
using System.Security.Cryptography;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // replace with per-faction tracker
    public static ResourceManager Instance;

    public float ResearchPoints;

    public float Resource = 0;

    private void Awake()
    {
        Instance = this;

        GameTick.onDay += GainResearchPoint;
        GameTick.onDay += GenerateResource;
    }
    private void GenerateResource()
    {
        Resource += 2.5f;
    }

    private void GainResearchPoint()
    {
        ResearchPoints += 1.5f;
        TechTree.instance.AddResearchPoints(ResearchPoints);
        ResearchPoints = 0;
    }



}
