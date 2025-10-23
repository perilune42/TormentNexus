using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum TechNodeStatus
{
    Locked, Unlocked, Finished
}

public class TechTree : MonoBehaviour
{
    public Faction Faction;

    public static TechTree PlayerTechTree;

    public Dictionary<TechNode, TechNodeStatus> techNodeStatuses = new();
    public List<TechNode> startNodes;
    public TechLineRenderer techLineRenderer;

    Queue<TechLineRenderer> lineList = new();

    public TechNode currentlyResearching;
    public float accumulatedPoints;

    private void Awake()
    {
        techLineRenderer = GetComponentInChildren<TechLineRenderer>();
        if (Faction.isPlayer) PlayerTechTree = this;

        Faction.TechTree = this;

        if (!Faction.isPlayer)
        {
            transform.position = new Vector3(0, 10000, 0);
        }
    }

    private void Start()
    {
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (TechNode node in nodes)
        {
            node.faction = Faction;
            LockNode(node);
            foreach (var prereq in node.prereqs)
            {
                TechLineRenderer outline = Instantiate(techLineRenderer, transform.GetChild(1).GetChild(0), false);
                int siblingIndex = techLineRenderer.transform.GetSiblingIndex();  //Sibling Index of techlinerenderer
                outline.transform.SetSiblingIndex(siblingIndex); //Makes sure it renders before buttons
                outline.CreateLine(node.transform.position, prereq.transform.position, Color.black);
                RectTransform rt = outline.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(1, 16); //Sets line size to be width 16
                outline.name = "Line Outline";


                TechLineRenderer clone = Instantiate(techLineRenderer, transform.GetChild(1).GetChild(0), false);
                lineList.Enqueue(clone);
                clone.transform.SetSiblingIndex(siblingIndex + 1); //Makes sure it renders before buttons and after outline
                clone.name = "Locked Line";
                clone.CreateLine(node.transform.position, prereq.transform.position, Color.red);
            }
        }
        foreach (TechNode node in startNodes)
        {
            UnlockNode(node);
        }
    }



    public void FinishNode(TechNode targetNode)
    {
        int lineListLen = lineList.Count;

        techNodeStatuses[targetNode] = TechNodeStatus.Finished;

        currentlyResearching.progressBar.SetLevel(1);
        currentlyResearching.progressText.text = $"{currentlyResearching.cost:0.0}/{currentlyResearching.cost}";

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
            foreach (var prereq in node.prereqs)
            {
                if (techNodeStatuses[prereq] == TechNodeStatus.Finished)
                {
                    int siblingIndex = techLineRenderer.transform.GetSiblingIndex();  //Sibling Index of techlinerenderer

                    TechLineRenderer clone = Instantiate(techLineRenderer, transform.GetChild(1).GetChild(0), false);
                    lineList.Enqueue(clone);
                    clone.transform.SetSiblingIndex(siblingIndex + 1); //Makes sure it renders before buttons and after outline
                    clone.name = "Unlocked Line";
                    Color researchBlueColor = new Color(0.21176470588f, 0.57647058823f, 0.95686274509f); //Blue
                    clone.CreateLine(node.transform.position, prereq.transform.position, researchBlueColor);
                }
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

        for (int i = 0; i < lineListLen; i++)
        {
            Destroy(lineList.Dequeue());
        }
    }

    public void StartResearch(TechNode targetNode)
    {

        currentlyResearching = targetNode;
        // stored resech fills up at most half of new research
        accumulatedPoints = Mathf.Min(currentlyResearching.cost / 2, accumulatedPoints);
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (TechNode node in nodes)
        {
            if (techNodeStatuses[node] == TechNodeStatus.Unlocked)
            {
                LockNode(node);
            }
        }
        UnlockNode(targetNode);
        targetNode.button.interactable = false;
        targetNode.button.image.color = Color.yellow;
        UpdateBar();
    }

    public void AddResearchPoints(float points)
    {
        accumulatedPoints += points;
        if (currentlyResearching != null && accumulatedPoints >= currentlyResearching.cost)
        {
            accumulatedPoints -= currentlyResearching.cost;
            FinishNode(currentlyResearching);
            currentlyResearching = null;
            accumulatedPoints = 0;
        }
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (currentlyResearching != null)
        {
            //currentlyResearching.progressBar.SetLevel(accumulatedPoints / currentlyResearching.cost);
            currentlyResearching.progressBar.SetLevel(accumulatedPoints / currentlyResearching.cost);
            currentlyResearching.progressText.text = $"{accumulatedPoints:0.0}/{currentlyResearching.cost}";
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

    public float GetCurrentTechProgress()
    {
        if (currentlyResearching == null) return -1f;
        else
        {
            return accumulatedPoints / currentlyResearching.cost;
        }
    }

}
