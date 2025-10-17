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
    }

    private void Start()
    {
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (TechNode node in nodes)
        {
            LockNode(node);
        }
        foreach (TechNode node in startNodes)
        {
            UnlockNode(node);
        }
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

        foreach (TechNode node in startNodes)
        {
            if (techNodeStatuses[node] != TechNodeStatus.Finished)
            {
                UnlockNode(node);
            }
        }

        targetNode.Finish();
    }

    public void StartResearch(TechNode targetNode)
    {

        currentlyResearching = targetNode;
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (TechNode node in nodes)
        {
            if (techNodeStatuses[node] == TechNodeStatus.Unlocked)
            {
                LockNode(node);
            }
        }
        UnlockNode(targetNode);
        targetNode.setInteractable(false);
        targetNode.setButtonColor(Color.yellow);
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
    //Interactable to true, status to unlocked
    public void UnlockNode(TechNode targetNode)
    {
        techNodeStatuses[targetNode] = TechNodeStatus.Unlocked;
        targetNode.Unlock();
    }
    
    //Interactable to false, status to locked
    public void LockNode(TechNode targetNode) 
    {
        techNodeStatuses[targetNode] = TechNodeStatus.Locked;
        targetNode.Lock();
    }

}
