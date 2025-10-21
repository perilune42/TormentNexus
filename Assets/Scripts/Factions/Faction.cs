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
}
