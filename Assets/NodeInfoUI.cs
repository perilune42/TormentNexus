using TMPro;
using UnityEngine;

public class NodeInfoUI : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] TMP_Text nodeNameText;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        PlayerControl.Instance.onSelectNode += (MapNode node) =>
        {
            SetVisible(node != null);
            if (node != null)
            {
                nodeNameText.text = node.Name;
            }
        };


    }

    private void SetVisible(bool visible)
    {
        canvas.enabled = visible;
    }
}
