using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour
{
    public static MapNode Hovered;
    public static MapNode Selected;

    // Info
    public string Name;
    public Faction Owner;

    // Editor Refs
    public List<MapNode> Neighbors;
    public TextMeshPro NameTextMeshPro;

    // Self Refs
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        AssignOwnRefs();
    }

    // TODO - Double draws, fix
    // TODO - Also draw in editor
    public void DrawNode()
    {
        AssignOwnRefs(); 

        // Name text
        NameTextMeshPro.text = Name;

        // Faction color
        if (Owner != null) spriteRenderer.color = Owner.FactionColor;

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

    private void OnMouseExit()
    {
        Hovered = null;
    }

    private void OnMouseEnter()
    {
        Hovered = this;
    }

    private void OnMouseDown()
    {
        Selected = this;

        // Debug
        spriteRenderer.color = FactionManager.instance.playerFaction.FactionColor;
    }

    private void AssignOwnRefs()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
