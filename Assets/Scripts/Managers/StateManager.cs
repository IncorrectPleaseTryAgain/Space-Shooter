using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{

    public static StateManager instance;
    GameStates state;

    // Unity Built-In Methods
    private void Awake()
    {
        instance = this;
    }
  
    public static event Action<GameStates> OnGameStateChanged;
    public void UpdateGameState(GameStates newState)
    {
        Debug.Log("State Changed: " + newState);
        state = newState;

        switch (newState)
        {
            case GameStates.MainMenu:
                MainSceneHandler();
                break;
            case GameStates.StartGame:
                StartSceneHandler();
                break;
            case GameStates.Settings:
                SettingsSceneHandler();
                break;
            case GameStates.Credits:
                CreditsSceneHandler();
                break;
            case GameStates.Pause:
                PauseStateHandler();
                break;
            case GameStates.Play:
                PlayStateHandler();
                break;
            case GameStates.Restart:
                RestartStateHandler();
                break;
            case GameStates.RoundStart:
                RoundStartStateHandler();
                break;
            case GameStates.EnemyKilled:
                EnemyKilledStateHandler();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void EnemyKilledStateHandler()
    {
        
    }

    private void RoundStartStateHandler()
    {
        
    }

    private void RestartStateHandler()
    {
        SceneManager.LoadScene(SceneNames.Game);
    }

    private void PlayStateHandler()
    {
        IsPaused = false;
    }

    private void PauseStateHandler()
    {
        IsPaused = true;
    }

    void CreditsSceneHandler()
    {
        if (SceneManager.GetActiveScene().name != SceneNames.Credits)
        {
            Debug.Log("LoadScene: Credits");
            SceneManager.LoadScene(SceneNames.Credits);
        }
    }

    void SettingsSceneHandler()
    {
        if (SceneManager.GetActiveScene().name != SceneNames.Settings)
        {
            Debug.Log("LoadScene: Settings");
            SceneManager.LoadScene(SceneNames.Settings);
        }
    }

    void StartSceneHandler()
    {
        if (SceneManager.GetActiveScene().name != SceneNames.Game)
        {
            Debug.Log("LoadScene: Start");
            SceneManager.LoadScene(SceneNames.Game);
        }
    }

    void MainSceneHandler()
    {
        if(SceneManager.GetActiveScene().name != SceneNames.MainMenu)
        {
            Debug.Log("LoadScene: Main");
            SceneManager.LoadScene(SceneNames.MainMenu);
        }
    }

    bool _isPaused = false;
    public bool IsPaused {
        get { return _isPaused; }
        set { _isPaused = value; }
    }
}

public enum GameStates
{
    MainMenu,
    StartGame,
    Settings,
    Credits,
    Pause,
    Play,
    Dead,
    Restart,
    RoundStart,
    EnemyKilled,
}
