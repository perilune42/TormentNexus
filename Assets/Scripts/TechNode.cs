using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<TechNode> prereqs;
    Button button;
    TMP_Text buttonText;
    public float cost;



    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable == false) return;
        buttonText.rectTransform.Translate(Vector3.down * 13);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable == false) return;
        buttonText.rectTransform.Translate(Vector3.up * 13);
    }
}
