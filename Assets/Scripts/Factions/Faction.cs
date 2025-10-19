using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Faction : MonoBehaviour
{
    public string FactionName;
    public Color FactionColor;
    public Sprite Flag;

    public ResourceManager Resource;
    public List<Unit> BuildableUnits;
    [HideInInspector] public List<Ability> Abilities;
    [SerializeField] Transform abilitiesList;

    public List<Unit> AllUnits;
    public List<MapNode> AllNodes;

    [HideInInspector] public AIControl AIControl;

    private void Awake()
    {
        Abilities = abilitiesList.GetComponentsInChildren<Ability>().ToList();
        foreach (Ability ability in Abilities)
        {
            ability.GiveToFaction(this);
        }

        AIControl = GetComponentInChildren<AIControl>();
        if (AIControl != null)
        {
            AIControl.SetToFaction(this);
        }

    }
}
