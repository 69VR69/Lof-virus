using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    [Header("Tiles")]
    [SerializeField] private Transform _levelEnvironment;
    [SerializeField] private TileScript _walkableTile;
    [SerializeField] private TileScript _wallTile;

    private List<TileScript> _levelTiles = new();
    public List<TileScript> LevelTiles => _levelTiles;

    [Header("Poti Bonomm")]
    [SerializeField] private PotiBonommScript _potiBonomm;
    private List<PotiBonommScript> _potiBonommList = new List<PotiBonommScript>();

    [Header("Levels")]
    [SerializeField] private ScriptableLevel[] _level;

    [Header("Misc")]
    // [SerializeField] private bool _debug = false;
    [SerializeField] private float _distanceBetweenTiles = 1f;
    public float DistanceBetweenTiles => _distanceBetweenTiles;

    public int CurrentLevel { get; private set; } = 0;
    public int LevelNumber => _level.Length;

    void Start()
    {
        GenerateLevel(0);
    }


    public void GenerateLevel(int levelNumber)
    {
        if (_level.Length <= 0)
        {
            Debug.LogError("No level found in TileManager");
            return;
        }

        if (levelNumber >= _level.Length)
        {
            Debug.LogError("Level number is too high");
            return;
        }

        DestroyLevel();
        CurrentLevel = levelNumber;
        var level = _level[levelNumber].Level;

        int height = 0;
        foreach (var line in level.GetLines())
        {
            for (int i = 0; i < line.Length; i++)
            {
                var pos = new Vector2(i, height);

                // foreach character in line
                switch (line[(int)pos.x])
                {
                    case '_':
                        CreateWalkableTile(pos);
                        break;
                    case ' ':
                        // Leave a space
                        break;
                    case '#':
                        CreateWall(pos);
                        break;
                    case 'o':
                        CreateWalkableTile(pos);
                        CreatePotiBonomm(pos);
                        break;
                    default:
                        throw new System.Exception("Not recognized tile in TileManager");
                }
            }
            height++;
        }
        
    }

    public void DestroyLevel()
    {
        _levelEnvironment.DestroyChildren();
        _levelTiles.Clear();
        _potiBonommList.Clear();
    }

    private void CreateWalkableTile(Vector2 position)
    {
        var tile = Instantiate(_walkableTile, position.ToTilePosition(_distanceBetweenTiles), Quaternion.identity, _levelEnvironment);
        tile.X = (int)position.x;
        tile.Y = (int)position.y;

        _levelTiles.Add(tile);
    }

    private void CreatePotiBonomm(Vector2 position)
    {
        var potiBonomm = Instantiate(_potiBonomm, position.ToPotiBonommPosition(_distanceBetweenTiles), Quaternion.identity, _levelEnvironment);
        _potiBonomm.SetPosition((int)position.x, (int)position.y);
        _potiBonommList.Add(potiBonomm);
    }

    private void CreateWall(Vector2 position)
    {
        var wall = Instantiate(_wallTile, position.ToWallPosition(_distanceBetweenTiles), Quaternion.identity, _levelEnvironment);
        wall.X = (int)position.x;
        wall.Y = (int)position.y;

        _levelTiles.Add(wall);
    }

    public bool CheckMove(Vector2 move)
    {
        var tile = _levelTiles.Where(t => t.X == move.x && t.Y == move.y).First();
        
        return tile != null && tile.IsWalkable;
    }
}
