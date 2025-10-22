using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public List<TechNode> prereqs;
    public Button button;
    TMP_Text buttonText;
    Image image;
    public ProgressBar progressBar;
    public float cost;
    public string textToShow;
    public Unit unit;
    public Unit replacingUnit;
    public Ability ability;
    public Ability replacingAbility;
    public Faction faction;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        buttonText.text = textToShow;
        image = GetComponent<Image>();
        progressBar.SetVisible(false);
        progressBar.SetLevel(0);
    }

    public void SetFaction(Faction faction)
    {
        this.faction = faction;
    }

    public void Lock()
    {
        button.interactable = false;
        image.color = Color.gray;
    }

    public void Unlock()
    {
        button.interactable = true;
        image.color = new Color(0.8f, 0.8f, 0.8f);
    }

    public void Finish()
    {
        button.interactable = false;
        button.GetComponent<Image>().color = Color.green;
        if (unit != null)
        {
            faction.BuildableUnits.Add(unit);
        }
        if (replacingUnit != null)
        {
            Unit old = faction.BuildableUnits.Find((unit) => unit.Name == replacingUnit.Name);
            faction.BuildableUnits.Remove(old);
        }
        if (ability != null)
        {
            faction.AddAbility(ability);
        }
        if (replacingAbility != null)
        {
            faction.RemoveAbility(replacingAbility);
        }
        if (faction.isPlayer) BuildMenu.Instance.UpdateBuildables();
    }

    public void Select()
    {
        if (TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        progressBar.SetVisible(true);
        TechTree.PlayerTechTree.StartResearch(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null) {
            return;
        }; //Play error sound?
        buttonText.rectTransform.Translate(Vector3.down * 13);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        ; //Play error sound?  
        buttonText.rectTransform.Translate(Vector3.up * 13);
    }

    public void OnPointerEnter()
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        image.color = Color.white;
    }

    public void OnPointerExit()
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        image.color = new Color(0.8f, 0.8f, 0.8f);
    }
}
