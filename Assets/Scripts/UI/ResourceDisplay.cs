using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text resourceText, resourceGainText, researchText;
    
    private void Update()
    {
        UpdateDisplay();
    }

    


    private void UpdateDisplay()
    {
        resourceText.text = $"{FactionManager.instance.playerFaction.Resource.ResourceAmount}";
        resourceGainText.text = $"(+{FactionManager.instance.playerFaction.Resource.ResourceGeneration()})";
        researchText.text = $"+{FactionManager.instance.playerFaction.Resource.ResearchPoints}/Day";
    }

}
