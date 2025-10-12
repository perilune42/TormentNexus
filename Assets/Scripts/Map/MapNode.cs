using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    // Info
    public string Name;
    public Faction Owner;

    // Editor Refs
    public List<MapNode> Neighbors;
    public TextMeshPro NameTextMeshPro;

    // Self Refs
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    public Unit ContainedUnit = null;

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

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayerControl.Instance.HoverNode(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayerControl.Instance.HoverNode(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PlayerControl.Instance.SelectNode(this);

            // Debug
            // spriteRenderer.color = FactionManager.instance.playerFaction.FactionColor;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PlayerControl.Instance.RClickNode(this);
        }
    }

    private void AssignOwnRefs()
    {
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public bool IsAdjacent(MapNode neighbor)
    {
        return Neighbors.Contains(neighbor);
    }
}
