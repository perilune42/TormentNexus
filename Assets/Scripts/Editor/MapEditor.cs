using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default fields (value, etc.)
        DrawDefaultInspector();

        // Reference to the target script
        Map map = (Map)target;

        // Add a button
        if (GUILayout.Button("Connect missing neighbors"))
        {
            foreach (MapNode node in map.Nodes)
            {
                foreach (MapNode neighbor in node.Neighbors)
                {
                    if (!neighbor.Neighbors.Contains(node))
                    {
                        neighbor.Neighbors.Add(node);
                        EditorUtility.SetDirty(neighbor);
                    }
                }
            }
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                map.gameObject.scene
            );
        }
    }
}