using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
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
    public TechNode firstNode;

    private void Awake()
    {
        instance = this;
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (var node in nodes)
        {
            techNodeStatuses.Add(node, TechNodeStatus.Locked);
            node.Lock();
        }
        firstNode.Unlock();

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

}
