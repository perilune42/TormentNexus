using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TMP_Text nodeNameText;
    [SerializeField] Image nodeFlagImage;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        PlayerControl.Instance.onSelectNode += (MapNode node) =>
        {
            SetVisible(node != null);
            if (node != null)
            {
                nodeNameText.text = node.Name;
                nodeFlagImage.sprite = node.Owner.Flag;
            }
        };


    }

    private void SetVisible(bool visible)
    {
        canvas.enabled = visible;
    }
}
