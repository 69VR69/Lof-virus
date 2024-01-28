using Assets;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class PotiBonommScript : MonoBehaviour, IPositionable
{
    [SerializeField] private int _x; // Serialize Field IS IMPORTANT HERE !!! I don't know why but it is
    [SerializeField] private int _y;

    public int X { get => _x; set => _x = value; }

    public int Y { get => _y; set => _y = value; }
    public Particles Particles { get => _particles; set => _particles = value; }
    public DirectionScript DirectionScript { get => _directionScript; set => _directionScript = value; }
    public Vector2Int LaughDirection { get => _laughDirection; set => _laughDirection = value; }

    [SerializeField] private Vector2Int _laughDirection = new();
    [SerializeField] private bool _isLaughing = false;

    [SerializeField] private BobMovement _bobMovement;
    [SerializeField] private Clickable _clickable;

    [SerializeField] private Particles _particles;

    [SerializeField] private DirectionScript _directionScript;

    public void MovePosition(Vector2Int movement)
    {
        if (!TileManager.Instance.CheckMove(new(X + movement.x, Y + movement.y)) || !_isLaughing) {
            return;
        }

        X += movement.x;
        Y += movement.y;
    }

    public void MoveTo(Vector2Int pos)
    {
        if (pos.x != X || pos.y != Y) return;

        _bobMovement.SetRun(() =>
        {
            var v = new Vector3(pos.x, 0, pos.y);
            return v;
        });
        transform.DOLocalMove(pos.ToPotiBonommPosition(TileManager.Instance.DistanceBetweenTiles), GameManager.Instance.AnimationTime);
    }

    public void LaughTo(Vector2Int dir)
    {
        if (!GameManager.Instance.State.Equals(GameManager.GameState.WaitingForMakeLaugh)) return;

        MakeLaugh(dir);
        
        GameManager.Instance.ChangeState(GameManager.GameState.StartOfTurn);
    }

    public void LaughRight()
    {
        LaughTo(new(1, 0));
    }

    public void LaughLeft()
    {
        LaughTo(new(-1, 0));
    }

    public void LaughUp()
    {
        LaughTo(new(0, 1));
    }

    public void LaughDown()
    {
        LaughTo(new(0, -1));
    }

    public void MakeLaugh(Vector2Int direction)
    {
        if (_isLaughing) return;

        _isLaughing = true;
        LaughDirection = direction;

        Particles.Play();

        GameManager.Instance.OnGameStateChanged.AddListener(
            gameState =>
            {
                if (gameState == GameManager.GameState.StartOfTurn)
                {
                    TriggerLaugh();
                    MovePosition(LaughDirection);
                }
                else if (gameState == GameManager.GameState.AnimationTime)
                {
                    MoveTo(new(X, Y));
                }
                else if (gameState == GameManager.GameState.EndOfTurn)
                {
                }
            }
            );
    }

    private void TriggerLaugh()
    {
        var seenBonomms = TileManager.Instance.GetSeenBonommsFrom(new Vector2(X, Y));

        foreach (var bonomm in seenBonomms)
        {
            var x = bonomm.X - X;
            var y = bonomm.Y - Y;

            // Normalize x and y
            if (x != 0) x /= Mathf.Abs(x);
            if (y != 0) y /= Mathf.Abs(y);

            var dir = new Vector2Int(
                x, 
                y
                );
            bonomm.MakeLaugh(dir);
        }
    }
}
