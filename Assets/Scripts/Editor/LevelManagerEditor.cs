using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileManager levelManager = target.GetComponent<TileManager>();
        
        if (Application.isPlaying && GUILayout.Button("Generate Next Level"))
        {
            var level = (levelManager.CurrentLevel + 1) % levelManager.LevelNumber;
            Debug.Log("Generating next level " + level);
            levelManager.GenerateLevel(level);
        }

        if (Application.isPlaying && GUILayout.Button("Regenerate Level"))
        {
            Debug.Log("Regenerating level");
            levelManager.GenerateLevel(levelManager.CurrentLevel);

        }
    }
}
