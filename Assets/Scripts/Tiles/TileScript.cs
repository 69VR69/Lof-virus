using DG.Tweening;
using UnityEngine;

public class TileScript : MonoBehaviour, IPositionable
{
    // [Header("Position")]
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }

    [Header("Type")]
    [SerializeField] private bool _isWalkable;
    public bool IsWalkable => _isWalkable;

    // [SerializeField] private bool _isWall; // May be useless
    // public bool IsWall => _isWall; // May be useless

    [Header("Animation")]
    [SerializeField] private bool _animateGrow = true;
    [SerializeField] private float _animationDuration = 0.5f;

    void Start()
    {
        if (_animateGrow)
        {
            var localScale = transform.localScale;
            transform.localScale = Vector3.zero;
            transform.DOScale(localScale, _animationDuration).SetEase(Ease.OutBack);
        }
    }
}
