using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechNode : MonoBehaviour
{
    public List<TechNode> prereqs;
    public Button button;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Image buttomImage;
    public ProgressBar progressBar;
    public TMP_Text progressText;
    public float cost;
    public string textToShow;
    public Unit unit;
    public Unit replacingUnit;
    public Ability ability;
    public Ability replacingAbility;
    public Faction faction;

    [SerializeField] Image unlockIcon;
    [SerializeField] TMP_Text unlockText;

    private void Awake()
    {
        buttonText.text = textToShow;
        // progressBar.SetVisible(false);
        progressBar.SetLevel(0);
        progressText.text = $"0/{cost}";
    }

    public void SetFaction(Faction faction)
    {
        this.faction = faction;
    }

    public void Lock()
    {
        button.interactable = false;
        buttomImage.color = new Color(0.3f, 0.3f, 0.3f);
    }

    public void Unlock()
    {
        button.interactable = true;
        buttomImage.color = new Color(0.9f, 0.9f, 0.9f);
    }

    public void Finish()
    {
        button.interactable = false;
        buttomImage.color = Color.green;
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
        Debug.Log("pressed");
        if (TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        progressBar.SetVisible(true);
        TechTree.PlayerTechTree.StartResearch(this);
        /*
        buttonText.rectTransform.Translate(Vector3.down * 13);
        StartCoroutine(RaiseButton());
        */
    }



    public void OnPointerDown()
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null) {
            return;
        }; //Play error sound?
        buttonText.rectTransform.Translate(Vector3.down * 13);
    }



    public void OnPointerUp()
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
        buttomImage.color = Color.white;
    }

    public void OnPointerExit()
    {
        if (button.interactable == false || TechTree.PlayerTechTree.currentlyResearching != null)
        {
            return;
        }
        buttomImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

    private void OnValidate()
    {
        gameObject.name = textToShow;
        buttonText.text = textToShow;
        ShowUnlocks();
    }

    private void ShowUnlocks()
    {
        if (ability != null)
        {
            unlockIcon.sprite = ability.icon;
            unlockText.text = ability.Name;
        }
        else if (unit != null)
        {
            unlockIcon.sprite = unit.Icon;
            unlockText.text = unit.Name;
        }
        else
        {
            unlockIcon.enabled = false;
            unlockText.enabled = false;
        }
    }
}
