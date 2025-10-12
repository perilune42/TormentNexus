using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public List<MapNode> Nodes;

    // Undirected edges, order doesn't matter for our purposes (but does for the pair structure so check both directions)
    public List<(MapNode, MapNode)> Edges;

    private void Awake()
    {
        Edges = new List<(MapNode, MapNode)> ();
    }

    private void Start()
    {
        foreach (MapNode node in Nodes)
        {
            foreach (MapNode neighbor in node.Neighbors)
            {
                if (!Edges.Contains((node, neighbor)) && !Edges.Contains((neighbor, node)))
                {
                    Edges.Add((node, neighbor));
                }
            }
        }

        DrawMap();
    }

    // TODO - Double draws lines, fix, potentially explore graph marking edges as drawn?
    private void DrawMap()
    {
        foreach (MapNode node in Nodes)
        {
            node.DrawNode();
        }
    }

    private void OnValidate()
    {
        Nodes = GetComponentsInChildren<MapNode>().ToList();
        DrawMap();
    }
}
