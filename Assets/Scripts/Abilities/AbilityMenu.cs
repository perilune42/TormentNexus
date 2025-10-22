using UnityEditor.Playables;
using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    
    [SerializeField] AbilityButton buttonTemplate;
    public static AbilityMenu Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RedrawButtons();
    }

    public void RedrawButtons()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
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
