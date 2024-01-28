using System;
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

    private Transform _bonommParent;

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

    public List<PotiBonommScript> PotiBonommList { get => _potiBonommList; set => _potiBonommList = value; }

    void Start()
    {
        GenerateLevel(0);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateLevel(CurrentLevel);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            var level = (CurrentLevel + 1) % LevelNumber;

            GenerateLevel(level);
        }
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
        PotiBonommList.Clear();
        GameManager.Instance.ChangeState(GameManager.GameState.WaitingForMakeLaugh);

        var parent = Instantiate(new GameObject(), _levelEnvironment);
        parent.name = "Bonomms";
        _bonommParent = parent.transform;
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
        var potiBonomm = Instantiate(_potiBonomm, position.ToTilePosition(_distanceBetweenTiles).Where(y:0), Quaternion.identity, _bonommParent);
        potiBonomm.X = (int)position.x;
        potiBonomm.Y = (int)position.y;

        PotiBonommList.Add(potiBonomm);
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
        var tiles = _levelTiles.Where(t => t.X == move.x && t.Y == move.y).ToList();
        if (tiles.Count == 0) return false;
        var tile = tiles.First();
        var isWalkableAndAvailable = tile.IsWalkable && PotiBonommList.Where(b => b.X == tile.X && b.Y == tile.Y).ToList().Count == 0;
        return isWalkableAndAvailable;
    }

    public List<PotiBonommScript> GetSeenBonommsFrom(Vector2 vector2)
    {
        // See every bonomm in the same line or column AND is not blocked by a wall
        var seenBonomms = new List<PotiBonommScript>();

        // Get all bonomms in the same line
        var bonommsInLine = PotiBonommList.Where(b => (b.X == vector2.x || b.Y == vector2.y) && (b.X != vector2.x || b.Y != vector2.y)).ToList();

        seenBonomms = bonommsInLine.Where(b =>
        {
            var minX = Mathf.Min(vector2.x, b.X);
            var maxX = Mathf.Max(vector2.x, b.X);
            var minY = Mathf.Min(vector2.y, b.Y);
            var maxY = Mathf.Max(vector2.y, b.Y);

            var concernedTiles = _levelTiles.Where(t => !t.IsWalkable && (t.X >= minX && t.X <= maxX && t.Y >= minY && t.Y <= maxY)).ToList();
            
            return concernedTiles.Count == 0;
        }).ToList();

        return seenBonomms;

    }
}
