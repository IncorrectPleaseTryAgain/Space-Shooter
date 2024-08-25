using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public void SetVolume(Slider slider) { }
    public void SetFullscreen(Toggle toggle) { }
    public void Done() 
    {
        StateManager.instance.UpdateGameState(GameStates.MainMenu);
    }
}
