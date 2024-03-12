using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerStatsManager))]
public class PlayerStatsCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PlayerStatsManager playerStatsManager = (PlayerStatsManager)target;
        if (GUILayout.Button("Clear"))
        {
            playerStatsManager.Clear();
        }
    }
}
