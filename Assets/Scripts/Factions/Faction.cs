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

    private void Awake()
    {
        Abilities = abilitiesList.GetComponentsInChildren<Ability>().ToList();
        foreach (Ability ability in Abilities)
        {
            ability.GiveToFaction(this);
        }

    }
}
