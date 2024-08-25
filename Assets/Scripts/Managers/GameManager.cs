using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject deathScreen;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject enemy;
    [SerializeField]
    TMP_Text round;

    int currentRound;
    int numEnemies;

    private void Awake()
    {
        StateManager.OnGameStateChanged += OnGameStateChangedHandler;
        currentRound = 0;
    }

    private void OnDestroy()
    {
        StateManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }
    private void Start()
    {
        round.text = currentRound.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StateManager.instance.IsPaused) { StateManager.instance.UpdateGameState(GameStates.Play); }
            else if (!StateManager.instance.IsPaused) { StateManager.instance.UpdateGameState(GameStates.Pause); }
            //Debug.Log("Pause Toggle: " + StateManager.instance.IsPaused);
        }

        if (numEnemies <= 0)
        {
            currentRound++;
            round.text = currentRound.ToString();
            StateManager.instance.UpdateGameState(GameStates.RoundStart);
        }
    }

    private void OnGameStateChangedHandler(GameStates state)
    {
        switch (state)
        {
            case GameStates.StartGame:
                StateManager.instance.UpdateGameState(GameStates.Play);
                break;
            case GameStates.Play:
                pauseMenu.SetActive(false);
                deathScreen.SetActive(false);
                NewRoundBegin();
                break;
            case GameStates.Pause:
                pauseMenu.SetActive(true);
                deathScreen.SetActive(false);
                break;
            case GameStates.Dead:
                deathScreen.SetActive(true);
                pauseMenu.SetActive(false);
                break;
            case GameStates.RoundStart:
                NewRoundBegin();
                break;
            case GameStates.EnemyKilled:
                numEnemies--;
                break;
        }
    }

    private void NewRoundBegin()
    {
        numEnemies = currentRound;

        for (int i = 0; i < numEnemies; i++)
        {
            Instantiate(enemy, RandomPosiiton(), RandomRotation());
        }

        Vector3 RandomPosiiton()
        {
            int x = UnityEngine.Random.Range(-5, 5);
            x = (x <= 0) ? (x - 10) : (x + 10);
            int y = UnityEngine.Random.Range(-5, 5);
            y = (y <= 0) ? (y - 10) : (y + 10);

            return new Vector3(player.transform.position.x + x, player.transform.position.y + y, 0);
        }

        Quaternion RandomRotation()
        {
            return Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
        }

        //Debug.Log("New Round Begin! " + numEnemies);
    }

    public void SetVolume(UnityEngine.UI.Slider slider) {  }
    public void SetFullscreen(UnityEngine.UI.Toggle toggle) { }

    public void Resume() { StateManager.instance.UpdateGameState(GameStates.Play); }
    public void MainMenu() { StateManager.instance.UpdateGameState(GameStates.MainMenu); }
    public void Restart() { StateManager.instance.UpdateGameState(GameStates.Restart); }
}
