using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.MainMenu, transform, 0.1f, true);
    }
    public void StartGame() 
    {
        StateManager.instance.UpdateGameState(GameStates.StartGame);
    }
    public void Settings() 
    {
        StateManager.instance.UpdateGameState(GameStates.Settings);
    }
    public void Credits() 
    {
        StateManager.instance.UpdateGameState(GameStates.Credits);
    }
    public void Exit() 
    { 
        Application.Quit();
    }
}
