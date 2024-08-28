using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private void Awake()
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.Settings ,transform, 1f, true);
    }

    public void SetVolume(Slider slider) { }
    public void SetFullscreen(Toggle toggle) { }
    public void Done() 
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.Button, transform, 1.5f);
        StateManager.instance.UpdateGameState(GameStates.MainMenu);
    }
}
