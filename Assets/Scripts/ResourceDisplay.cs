using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    
    private void Awake()
    {
        GameTick.onDay += GenerateResource;
        GameTick.onDay += UpdateDisplay;
    }

    float resource = 0;
    // dummy test function
    private void GenerateResource()
    {
        resource += 2.5f;
    }

    private void UpdateDisplay()
    {
        text.text = $"Resource: {resource}";
    }

}
