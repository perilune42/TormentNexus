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
    public TechLineRenderer techLineRenderer;

    Queue<TechLineRenderer> lineList = new();

    public TechNode currentlyResearching;
    public float accumulatedPoints;
    

    [SerializeField] TMP_Text progressText;

    private void Awake()
    {
        instance = this;
        techLineRenderer = GetComponentInChildren<TechLineRenderer>();
    }

    private void Start()
    {
        var nodes = GetComponentsInChildren<TechNode>();
        foreach (TechNode node in nodes)
        {
            LockNode(node);
            foreach (var prereq in node.prereqs)
            {
                TechLineRenderer outline = Instantiate(techLineRenderer, transform, false);
                int siblingIndex = techLineRenderer.transform.GetSiblingIndex();  //Sibling Index of techlinerenderer
                outline.transform.SetSiblingIndex(siblingIndex); //Makes sure it renders before buttons
                outline.CreateLine(node.transform.position, prereq.transform.position, Color.black); 
                RectTransform rt = outline.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(1, 16); //Sets line size to be width 10
                outline.name = "Line Outline";


                TechLineRenderer clone = Instantiate(techLineRenderer, transform, false);
                lineList.Enqueue(clone);
                clone.transform.SetSiblingIndex(siblingIndex+1); //Makes sure it renders before buttons and after outline
                clone.name = "Locked Line";
                clone.CreateLine(node.transform.position, prereq.transform.position, Color.red);
            }
        }
        foreach (TechNode node in startNodes)
        {
            UnlockNode(node);
        }
        drawConnections();
    }



    public void FinishNode(TechNode targetNode)
    {
        int lineListLen = lineList.Count;

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
                foreach (var prereq in node.prereqs)
                {
                    int siblingIndex = techLineRenderer.transform.GetSiblingIndex();  //Sibling Index of techlinerenderer

                    TechLineRenderer clone = Instantiate(techLineRenderer, transform, false);
                    lineList.Enqueue(clone);
                    clone.transform.SetSiblingIndex(siblingIndex + 1); //Makes sure it renders before buttons and after outline
                    clone.name = "Unlocked Line";
                    Color researchBlueColor = new Color(0.21176470588f, 0.57647058823f, 0.95686274509f);
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

    private void drawConnections()
    {
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
