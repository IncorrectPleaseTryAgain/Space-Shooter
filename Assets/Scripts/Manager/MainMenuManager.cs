using System;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text bestRound;
    [SerializeField] private TMP_Text bestTime;

    private string[] saveString;

    private void Awake() { StateManager.OnGameStateChanged += OnGameStateChangedHandler; }


    private void OnDestroy() { StateManager.OnGameStateChanged -= OnGameStateChangedHandler; }

    private void Start() { SaveManager.instance.LoadBestRound(bestRound, bestTime); }

    private void OnGameStateChangedHandler(GameStates states)
    {
        if(states == GameStates.SceneMainMenu) { SaveManager.instance.LoadBestRound(bestRound, bestTime); }
    }
}
