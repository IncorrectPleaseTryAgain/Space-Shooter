using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{ 
    public static StateManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private const string SCENE_MAIN_MENU = "MainMenu";
    private const string SCENE_GAME = "Game";
    private const string SCENE_SETTINGS = "Settings";
    private const string SCENE_CREDITS = "Credits";

    public static event Action<GameStates> OnGameStateChanged;
    public void UpdateGameState(GameStates newState)
    {
        // Debug.Log("State Changed: " + newState);
        switch (newState)
        {
            case GameStates.SceneMainMenu:
                SceneMainMenuHandler();
                break;
            case GameStates.SceneGame:
                SceneGameHandler();
                break;
            case GameStates.SceneSettings:
                SceneSettingsHandler();
                break;
            case GameStates.SceneCredits:
                SceneCreditsHandler();
                break;
            case GameStates.Exit:
                ExitHandler();
                break;
            case GameStates.GamePause:
                //GamePauseHandler();
                break;
            case GameStates.GameResume:
                //GameResumeHandler();
                break;
            case GameStates.GameRestart:
                //GameRestartHandler();
                break;
            case GameStates.NewRoundStart:
                //NewRoundStartHandler();
                break;
            case GameStates.PlayerDeath:
                //PlayerDeathHandler();
                break;
            case GameStates.EnemyDeath:
                //EnemyDeathHandler();
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }


    private void SceneMainMenuHandler() { SceneManager.LoadScene(SCENE_MAIN_MENU); }
    private void SceneGameHandler() { SceneManager.LoadScene(SCENE_GAME); }
    private void SceneSettingsHandler() { SceneManager.LoadScene(SCENE_SETTINGS); }
    private void SceneCreditsHandler() { SceneManager.LoadScene(SCENE_CREDITS); }
    private void ExitHandler() { Application.Quit(); }
    //private void GamePauseHandler() { }
    //private void GameResumeHandler() { }
    //private void GameRestartHandler() { }
    //private void NewRoundStartHandler() { }
    //private void PlayerDeathHandler() { }
    //private void EnemyDeathHandler() { }


}

public enum GameStates
{
    SceneMainMenu,
    SceneGame,
    SceneSettings,
    SceneCredits,
    Exit,

    GamePause,
    GameResume,
    GameRestart,
    NewRoundStart,

    PlayerDeath,
    EnemyDeath
}
