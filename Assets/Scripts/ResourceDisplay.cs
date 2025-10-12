using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    private void Awake()
    {
        GameTick.onTick += UpdateDisplay;
    }

    


    private void UpdateDisplay()
    {
        text.text = $"Resource: {FactionManager.instance.playerFaction.Resource.ResourceAmount}";
    }

}
