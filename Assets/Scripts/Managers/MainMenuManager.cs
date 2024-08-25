using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame() { StateManager.instance.UpdateGameState(GameStates.StartGame); }
    public void Settings() { StateManager.instance.UpdateGameState(GameStates.Settings); }
    public void Credits() { StateManager.instance.UpdateGameState(GameStates.Credits); }
    public void Exit() { Application.Quit(); }
}
