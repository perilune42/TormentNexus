using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public Faction Faction;


    public float ResearchPoints;

    public float ResourceAmount = 0;

    

    private const float ResourcePerMilitaryNode = 0.2f;
    private const float ResourcePerCapital = 1f;

    private const float ResearchPerScienceNode = 0.2f;
    private const float ResearchPerCapital = 1f;

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

    }

    public void SetFaction(Faction faction)
    {
        this.Faction = faction;
        if (!faction.isMajorFaction) return;
        GameTick.onDay += GainResearchPoint;
        GameTick.onDay += GenerateResource;
    }

    private void GenerateResource()
    {
        ResourceAmount = Mathf.Round((ResourceGeneration() + ResourceAmount) * 100f) / 100f;
    }

    public void ConsumeResource(float amount)
    {
        ResourceAmount = Mathf.Round((ResourceAmount - amount) * 100f) / 100f;
    }

    private void GainResearchPoint()
    {
        ResearchPoints = 0;
        if (Faction.AllNodes.Where((node) => node.Type == NodeType.Capital).Count() > 0)
        {
            ResearchPoints += ResearchPerCapital;
        }
        ResearchPoints += Faction.AllNodes.Where((node) => node.Type == NodeType.Science).Count() * ResearchPerScienceNode;
        Faction.TechTree.AddResearchPoints(ResearchPoints);
    }



}
