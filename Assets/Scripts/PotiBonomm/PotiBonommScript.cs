using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotiBonommScript : MonoBehaviour, IPositionable
{
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    public int X { get => _x; set => _x = value; }

    public int Y { get => _y; set => _y = value; }

    
    public void SetPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void MovePosition(Vector2 movement)
    {
        X += (int)movement.x;
        Y += (int)movement.y;
    }

    public void Move(Vector2 movement)
    {
        if (!TileManager.Instance.CheckMove(new(X + movement.x, Y + movement.y))) return;

        // TODO : Verification
        transform.DOMove((new Vector2(X + movement.x, Y + movement.y)).ToPotiBonommPosition(TileManager.Instance.DistanceBetweenTiles), GameManager.Instance.AnimationTime);
        MovePosition(movement);

        TriggerFear();
    }

    public void DebugMove()
    {
        Move(new Vector2(1, 0));
    }

    private void TriggerFear()
    {

    }
}
