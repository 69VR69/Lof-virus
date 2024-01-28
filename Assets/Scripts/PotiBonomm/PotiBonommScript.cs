using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PotiBonommScript : MonoBehaviour, IPositionable
{
    [SerializeField] private int _x; // Serialize Field IS IMPORTANT HERE !!! I don't know why but it is
    [SerializeField] private int _y;

    public int X { get => _x; set => _x = value; }

    public int Y { get => _y; set => _y = value; }

    [SerializeField] private Vector2Int _laughDirection = new();
    [SerializeField] private bool _isLaughing = false;

    void Start()
    {
        var clickable = GetComponent<Clickable>();
        clickable.OnClick.AddListener((e) => DebugMove());
    }
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
        transform.DOLocalMove(pos.ToPotiBonommPosition(TileManager.Instance.DistanceBetweenTiles), GameManager.Instance.AnimationTime);
    }

    public void DebugMove()
    {
        if (!GameManager.Instance.State.Equals(GameManager.GameState.WaitingForMakeLaugh)) return;

        MakeLaugh(new(1, 0));
        
        GameManager.Instance.ChangeState(GameManager.GameState.StartOfTurn);
    }

    public void MakeLaugh(Vector2Int direction)
    {
        if (_isLaughing) return;

        _isLaughing = true;
        _laughDirection = direction;

        GameManager.Instance.OnGameStateChanged.AddListener(
            gameState =>
            {
                if (gameState == GameManager.GameState.StartOfTurn)
                {
                    TriggerLaugh();
                    MovePosition(_laughDirection);
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
