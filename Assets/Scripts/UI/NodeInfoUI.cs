using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TMP_Text nodeNameText;

    [SerializeField] Image nodeFlagImage;
    [SerializeField] TMP_Text factionNameText;

    [SerializeField] TMP_Text nodeTypeText;
    [SerializeField] Image nodeTypeImage;

    [SerializeField] TMP_Text garText, infText;
    [SerializeField] ProgressBar garBar, infBar;

    Dictionary<NodeType, string> nodeTypeDescs = new()
    {
        [NodeType.Capital] = "Capital",
        [NodeType.City] = "City",
        [NodeType.Military] = "Military Base",
        [NodeType.Science] = "Research Center"
    };

    private void Awake()
    {
        canvas = GetComponent<Canvas>();

    }

    private void SetVisible(bool visible)
    {
        canvas.enabled = visible;
    }
    private void Update()
    {
        var node = PlayerControl.Instance.SelectedNode;
        SetVisible(node != null);
        if (node != null)
        {
            nodeNameText.text = node.Name;
            nodeFlagImage.sprite = node.Owner.Flag;
            factionNameText.text = node.Owner.FactionName;
            factionNameText.color = Color.Lerp(node.Owner.FactionColor, Color.white, 0.4f);
            nodeTypeImage.sprite = node.spriteRenderer.sprite;
            nodeTypeImage.color = node.Owner.FactionColor;
            nodeTypeText.color = Color.Lerp(node.Owner.FactionColor,Color.white,0.4f) ;
            nodeTypeText.text = nodeTypeDescs[node.Type];
            garText.text = $"{(int)node.GarrisonHealth}/{(int)node.MaxGarrisonHealth}";
            infText.text = $"{(int)node.InfrastructureHealth}/{(int)node.MaxInfrastructureHealth}";
            garBar.SetLevel(node.GarrisonHealth / node.MaxGarrisonHealth);
            garBar.SetLevel(node.InfrastructureHealth / node.MaxInfrastructureHealth);
        }

    }
}
