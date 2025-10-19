using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static IEnumerator DelayedCall(float time, Action func)
    {
        yield return new WaitForSeconds(time);
        func?.Invoke();
    }

    // onUpdate param: t (0 to 1)
    public static IEnumerator ContinuousCall(float time, Action<float> onUpdate, Action onEnd)
    {
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            onUpdate?.Invoke((Time.time - startTime) / time);
            yield return new WaitForEndOfFrame();
        }
        onEnd?.Invoke();
    }


    // shamelessly written by AI
    public static MapNode Pathfind(Unit fromUnit, MapNode to, Func<MapNode, bool> blacklist = null)
    {
        return PathfindConditional(fromUnit, (node) => node == to, blacklist);
    }



    public static MapNode PathfindConditional(Unit fromUnit, Func<MapNode, bool> condition, Func<MapNode, bool> blacklist = null)
    {
        MapNode from = fromUnit.CurrentNode;

        if (condition(from))
            return from;

        var queue = new Queue<MapNode>();
        var prev = new Dictionary<MapNode, MapNode>();
        var visited = new HashSet<MapNode>();

        queue.Enqueue(from);
        visited.Add(from);
        prev[from] = null;

        // --- BFS traversal ---
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in current.Neighbors)
            {
                if (blacklist != null && blacklist(neighbor)) continue;

                // avoid friendly collisions
                if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.Owner == fromUnit.Owner) continue;

                if (visited.Contains(neighbor))
                    continue;

                visited.Add(neighbor);
                prev[neighbor] = current;
                queue.Enqueue(neighbor);

                // Found destination early
                if (condition(neighbor))
                {
                    return GetNextNode(from, neighbor, prev);
                }
            }
        }

        // No path found
        return null;
    }

    private static MapNode GetNextNode(MapNode from, MapNode to, Dictionary<MapNode, MapNode> prev)
    {
        // Backtrack from `to` to find the path
        var current = to;
        var path = new Stack<MapNode>();

        while (current != null)
        {
            path.Push(current);
            current = prev[current];
        }

        // Path is now [from, ..., to]
        // Return the *next* node after `from`
        if (path.Count > 1 && path.Peek() == from)
        {
            path.Pop(); // remove "from"
            return path.Pop();
        }

        return null;
    }

    public static List<MapNode> GetNeighboringEnemies(MapNode node)
    {
        List<MapNode> neighboringEnemies = new();
        foreach (MapNode neighbor in node.Neighbors)
        {
            if (neighbor.Owner != node.Owner)
            {
                neighboringEnemies.Add(neighbor);
            }
        }
        return neighboringEnemies;
    }

    public static List<Unit> GetNeighboringEnemyUnits(MapNode node)
    {
        List<Unit> neighboringEnemies = new();
        foreach (MapNode neighbor in node.Neighbors)
        {
            if (neighbor.ContainedUnit != null && neighbor.ContainedUnit.Owner != node.Owner)
            {
                neighboringEnemies.Add(neighbor.ContainedUnit);
            }
        }
        return neighboringEnemies;
    }

}