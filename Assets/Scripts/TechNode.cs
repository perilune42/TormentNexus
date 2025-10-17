using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<TechNode> prereqs;
    Button button;
    TMP_Text buttonText;
    Image image;
    public float cost;
    public string textToShow;



    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
        buttonText.text = textToShow;
    }

    public void Lock()
    {
        button.interactable = false;
        image.color = Color.gray;
    }

    public void Unlock()
    {
        button.interactable = true;
        image.color = Color.white;
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
        if (button.interactable == false || TechTree.instance.currentlyResearching != null) {
            return;
        }; //Play error sound?
        buttonText.rectTransform.Translate(Vector3.down * 13);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable == false || TechTree.instance.currentlyResearching != null)
        {
            return;
        }
        ; //Play error sound?  
        buttonText.rectTransform.Translate(Vector3.up * 13);
    }

    public void setInteractable(bool b)
    {
        button.interactable = b;
    }

    public void setButtonColor(Color c)
    {
        image.color = c;
    }
}
