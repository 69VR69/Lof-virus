using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [Header("Game State")]
    [SerializeField] private GameState _gameState = GameState.WaitingForMakeLaugh;
    [SerializeField] private OnGameStateChanged _onGameStateChanged = new();
    public GameState State => _gameState;

    [Header("Stats")]
    [SerializeField] private float _animationTime = 1f;
    public float AnimationTime { get => _animationTime; private set => _animationTime = value; }
    public OnGameStateChanged OnGameStateChanged { get => _onGameStateChanged; set => _onGameStateChanged = value; }

    public void Start()
    {

    }

    public void Update()
    {
    }

    public void ChangeState(GameState newState)
    {
        _gameState = newState;

        switch (_gameState)
        {
            case GameState.WaitingForMakeLaugh:
                OnGameStateChanged.Invoke(GameState.WaitingForMakeLaugh);
                break;
            case GameState.StartOfTurn:
                OnGameStateChanged.Invoke(GameState.StartOfTurn);
                ChangeState(GameState.AnimationTime);
                break;
            case GameState.AnimationTime:
                OnGameStateChanged.Invoke(GameState.AnimationTime);
                this.DelayAction(() => {
                        if (_gameState == GameState.AnimationTime) ChangeState(GameState.EndOfTurn);
                    }, _animationTime);
                break;
            case GameState.EndOfTurn:
                OnGameStateChanged.Invoke(GameState.EndOfTurn);
                ChangeState(GameState.StartOfTurn);
                break;
            case GameState.GameOver:
                OnGameStateChanged.Invoke(GameState.GameOver);
                break;
            case GameState.Win:
                OnGameStateChanged.Invoke(GameState.Win);
                break;
            case GameState.Paused:      // May be useless
                OnGameStateChanged.Invoke(GameState.Paused);
                break;
            default:
                throw new Exception("GameState not recognized");
        }
    }


    public enum GameState
    {
        WaitingForMakeLaugh,
        // WaitingForPlaceFartBomb, // TODO
        StartOfTurn,
        AnimationTime,
        EndOfTurn,
        GameOver,
        Win,
        Paused
    }
}

#region Events

[Serializable] public class OnGameStateChanged : UnityEvent<GameManager.GameState> { }

#endregion