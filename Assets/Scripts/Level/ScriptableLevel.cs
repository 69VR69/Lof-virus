using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level", order = 1)]
public class ScriptableLevel : ScriptableObject
{
    [Header("Level info")]
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private string _description; // May be useless
    public string Description => _description;

    [SerializeField] private int _levelNumber;
    public int LevelNumber => _levelNumber;

    [Header("Level")]
    [Tooltip("Write the level here, use '_' for walkable, '#' for wall, '\\n' to make a new level line, and ' ' for nothing")]
    [SerializeField]
    [TextArea] private string _level;
    public string Level => _level;
    

}
