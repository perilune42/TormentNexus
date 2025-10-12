using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum TechNodeStatus
{
    Locked, Unlocked, Finished
}

public class TechTree : MonoBehaviour
{
    public static TechTree instance;
    public Dictionary<TechNode, TechNodeStatus> techNodeStatuses = new();
    public List<TechNode> startNodes;

    public TechNode currentlyResearching;
    public float accumulatedPoints;

    [SerializeField] TMP_Text progressText;

    private void Awake()
    {
        instance = this;
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (var node in nodes)
        {
            techNodeStatuses.Add(node, TechNodeStatus.Locked);
            node.Lock();
        }
        foreach (TechNode n in startNodes)
        {
            n.Unlock();
        }

    }

    public void UnlockNode(TechNode targetNode) 
    {
        techNodeStatuses[targetNode] = TechNodeStatus.Unlocked;
        targetNode.Unlock();
    }

    public void FinishNode(TechNode targetNode)
    {
        techNodeStatuses[targetNode] = TechNodeStatus.Finished;
        
        foreach (var node in techNodeStatuses.Keys.ToList())
        {
            if (techNodeStatuses[node] != TechNodeStatus.Locked)
            {
                continue;
            }
            bool allFinished = true;
            foreach (var prereq in node.prereqs)
            {
                if (techNodeStatuses[prereq] != TechNodeStatus.Finished)
                {
                    allFinished = false;
                }
            }
            if (allFinished)
            {
                UnlockNode(node);
            }
        }

        targetNode.Finish();
    }

    public void StartResearch(TechNode targetNode)
    {

        currentlyResearching = targetNode;
        UpdateText();
    }

    public void AddResearchPoints(float points)
    {
        if (currentlyResearching != null)
        {
            accumulatedPoints += points;

        }
        if (currentlyResearching != null && accumulatedPoints >= currentlyResearching.cost)
        {
            accumulatedPoints -= currentlyResearching.cost;
            FinishNode(currentlyResearching);
            currentlyResearching = null;
            accumulatedPoints = 0;
        }
        UpdateText();
    }

    private void UpdateText()
    {
        if (currentlyResearching != null)
        {
            progressText.text = $"Progress: {accumulatedPoints} / {currentlyResearching.cost}";
        }
        else
        {
            progressText.text = "No research selected";
        }
    }

}
