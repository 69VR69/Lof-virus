using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject _mainMenu;
    private bool _isPaused = false;

    public bool IsPaused => _isPaused;

    private void Start()
    {
        TogglePause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _mainMenu.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }
}
