using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public string FactionName;
    public Color FactionColor;
    public Sprite Flag;
    public MapNode Capital;

    public ResourceManager Resource;
    public TechTree TechTree;

    public List<Unit> BuildableUnits;
    [HideInInspector] public List<Ability> Abilities;
    [SerializeField] Transform abilitiesList;

    public List<Unit> AllUnits;
    public List<MapNode> AllNodes;

    [HideInInspector] public AIControl AIControl;
    [HideInInspector] public HateMeter HateMeter;
    public bool isMajorFaction = false;
    public bool isPlayer = false;

    private void Awake()
    {
        Abilities = abilitiesList.GetComponentsInChildren<Ability>().ToList();
        foreach (Ability ability in Abilities)
        {
            ability.GiveToFaction(this);
        }
        Abilities = Abilities.OrderBy(u => u.Type).ToList();

        AIControl = GetComponentInChildren<AIControl>();
        HateMeter = GetComponentInChildren<HateMeter>();

        if (isPlayer)
        {
            AIControl.gameObject.SetActive(false);
            HateMeter.gameObject.SetActive(false);
            AIControl = null;
            HateMeter = null;
        }

        if (AIControl != null)
        {
            AIControl.SetToFaction(this);
        }
        if (HateMeter != null)
        {
            HateMeter.SetToFaction(this);
        }


    }

    public void AddAbility(Ability template)
    {
        Ability newAbility = Instantiate(template, abilitiesList);
        Abilities.Add(newAbility);
        newAbility.GiveToFaction(this);
        Abilities = Abilities.OrderBy(u => u.Type).ToList();
        if (isPlayer)
        {
            AbilityMenu.Instance.RedrawButtons();
        }
    }

    public void RemoveAbility(Ability template)
    {
        var toRemove = Abilities.Find((ability) => ability.Name == template.Name);
        Abilities.Remove(toRemove);
        Destroy(toRemove.gameObject);
        Abilities = Abilities.OrderBy(u => u.Type).ToList();
        if (isPlayer)
        {
            AbilityMenu.Instance.RedrawButtons();
        }
    }
}
