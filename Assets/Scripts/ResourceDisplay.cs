using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    private void Awake()
    {
        GameTick.onDay += UpdateDisplay;
    }

    
    // dummy test function


    private void UpdateDisplay()
    {
        text.text = $"Resource: {ResourceManager.Instance.Resource}";
    }

}
