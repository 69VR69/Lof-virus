using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private bool _winFlag = false;

    private void Update()
    {
        if (State == GameState.Win && !_winFlag)
        {
            _winFlag = true;
            this.DelayAction(() => {
                var instance = TileManager.Instance;
                var level = (instance.CurrentLevel + 1) % instance.LevelNumber;
                instance.GenerateLevel(level);
                _winFlag = false;
            }, 2f);
        }
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
                if (CheckWin())
                {
                    this.DelayAction(() => ChangeState(GameState.Win), 1f);
                    return;
                }
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


    private bool CheckWin()
    {
        // If all potibonomms are laughing
        Debug.Log("CheckWin ? ");
        var potibonomms = TileManager.Instance.PotiBonommList;
        var win = potibonomms.Where(p => p.IsLaughing).ToList().Count == potibonomms.Count;
        if (win) Debug.Log("WIN :) ");
        return win;
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