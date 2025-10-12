using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    // Info
    public string Name;
    public Country Owner;

    // Editor Refs
    public List<MapNode> Neighbors;
    public TextMeshPro NameTextMeshPro;

    // Self Refs
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // TODO - Double draws, fix
    // TODO - Also draw in editor
    public void DrawNode()
    {
        // Name text
        NameTextMeshPro.text = Name;

        // Lines, currently scuffed
        lineRenderer.positionCount = Neighbors.Count * 2 + 1;
        lineRenderer.SetPosition(0, transform.position);

        int i = 1;
        foreach (MapNode neighbor in Neighbors)
        {
            lineRenderer.SetPosition(i, neighbor.transform.position);
            lineRenderer.SetPosition(i + 1, transform.position);
            i += 2;
        }
    }

    public void OnCapture()
    {

    }
}
