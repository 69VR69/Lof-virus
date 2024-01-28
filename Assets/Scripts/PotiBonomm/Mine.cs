using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Mine : MonoBehaviour, IPositionable
{
    private VisualEffect _fart;
    private Light _light;

    [SerializeField]
    private float _fartDelay = 1.5f;
    [SerializeField]
    private string _collisionTag;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private int _x;
    [SerializeField] private int _y;

    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _fart = GetComponent<VisualEffect>();
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        _audioSource.Stop();
        _fart.enabled = true;
        _fart.Stop();
        _light.enabled = true;

        var gameManager = GameManager.Instance;
        gameManager.OnGameStateChanged.AddListener(HandleMine);
    }

    private void HandleMine(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.StartOfTurn:
                break;
            case GameManager.GameState.EndOfTurn:

                if (CheckNextToPotibonomm())
                {
                    Explode();
                }    

                break;
            case GameManager.GameState.GameOver:
                break;
            case GameManager.GameState.Paused:
                break;
            case GameManager.GameState.WaitingForMakeLaugh:
                break;
            case GameManager.GameState.Win:
                break;
            default:
                break;
        }
    }

    private bool CheckNextToPotibonomm()
    {
        var adjacentBonomms = FindAdjacentPotiBonomms();
        Debug.LogError("adjacentBonomms.Count: " + adjacentBonomms.Count);

        return adjacentBonomms.Count > 0;
    }

    private List<PotiBonommScript> FindAdjacentPotiBonomms()
    {
        var tileManager = TileManager.Instance;
        var bonomms = tileManager.PotiBonommList;
        Predicate<PotiBonommScript> adjacentX = (PotiBonommScript b) => b.X == X + 1 || b.X == X - 1;
        Predicate<PotiBonommScript> adjacentY = (PotiBonommScript b) => b.Y == Y + 1 || b.Y == Y - 1;

        var adjacentBonomms = bonomms.FindAll(b => (adjacentX(b) && b.Y == Y) || (adjacentY(b) && b.X == X)).ToList();

        return adjacentBonomms;
    }

    public void Explode()
    {
        _fart.Play();
        _light.enabled = false;
        StartCoroutine("StopFart");

        var adjacentBonomms = FindAdjacentPotiBonomms();
        adjacentBonomms.ForEach(b => {
            // Make them laugh in the opposite direction
            var dir = new Vector2Int(b.X - X, b.Y - Y);
            b.LaughDirection = dir;
        });

        _audioSource.Play();
        this.DelayAction(() => Destroy(gameObject), 1.5f);

    }

    private IEnumerator StopFart()
    {
        yield return new WaitForSeconds(_fartDelay);

        _fart.Stop();
    }
}
