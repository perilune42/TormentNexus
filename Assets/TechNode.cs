using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechNode : MonoBehaviour
{
    public List<TechNode> prereqs;
    Button button;
    public float cost;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Lock()
    {
        button.interactable = false;
    }

    public void Unlock()
    {
        button.interactable = true;
    }

    public void Finish()
    {
        button.interactable = false;
        button.GetComponent<Image>().color = Color.green;
    }

    public void Select()
    {
        if (TechTree.instance.currentlyResearching != null)
        {
            return;
        }
        TechTree.instance.StartResearch(this);
    }

}
