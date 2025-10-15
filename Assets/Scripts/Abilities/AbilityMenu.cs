using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    
    [SerializeField] AbilityButton buttonTemplate;
    private void Start()
    {
        foreach (Ability ability in FactionManager.instance.playerFaction.Abilities)
        {
            CreateButtonForAbility(ability);
        }
        
    }

    private void CreateButtonForAbility(Ability ability)
    {
        var newButton = Instantiate(buttonTemplate, transform);
        newButton.SetAbility(ability);
    }


}
