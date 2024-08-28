using System;
//using System.Diagnostics;
using TMPro;
using UnityEngine;
//using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    TMP_Text clock;
    private Stopwatch stopwatch;

    bool playerIsAlive;

    float timer;
    AudioSource audioSource;
    public TimeSpan timeElapsed { get; private set; }

    private void Awake()
    {
        StateManager.OnGameStateChanged += OnGameStateChangedHandler;
        currentRound = 0;
        timer = UnityEngine.Random.Range(15, 30);

        playerIsAlive = true;
    }

    private void OnDestroy()
    {
        StateManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }
    private void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();

        round.text = currentRound.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (StateManager.instance.IsPaused) { StateManager.instance.UpdateGameState(GameStates.Resume); }
            else if (!StateManager.instance.IsPaused) { StateManager.instance.UpdateGameState(GameStates.Pause); }
            //Debug.Log("Pause Toggle: " + StateManager.instance.IsPaused);
        }

        if (numEnemies <= 0)
        {
            currentRound++;
            StateManager.instance.UpdateGameState(GameStates.RoundStart);
        }

       
        if (playerIsAlive)
        {
            if (timer <= 0f)
            {
                audioSource = SoundFXManager.instance.PlayRandomSoundFXClip(Sounds.instance.Game, transform, 0.1f);
                timer = UnityEngine.Random.Range(audioSource.clip.length + 30, 60);
                //UnityEngine.Debug.Log("New Song: " + audioSource);
            }
            timer -= Time.deltaTime;


            TimeSpan ts = stopwatch.Elapsed;
            string time = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            clock.text = time;
        }
    }

    private void OnGameStateChangedHandler(GameStates state)
    {
        switch (state)
        {
            case GameStates.StartGame:
                StateManager.instance.UpdateGameState(GameStates.Resume);
                break;
            case GameStates.Resume:
                ResumeHandler();
                break;
            case GameStates.Pause:
                PauseHandler();
                break;
            case GameStates.Dead:
                DeathHandler();
                break;
            case GameStates.RoundStart:
                NewRoundBegin();
                break;
            case GameStates.EnemyKilled:
                numEnemies--;
                break;
        }
    }

    private void PauseHandler()
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PauseMenuOpen, transform, 1f);
        if (audioSource != null) { audioSource.Pause(); }
        pauseMenu.SetActive(true);
        deathScreen.SetActive(false);
    }

    private void ResumeHandler()
    {
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.PauseMenuClose, transform, 1f);
        if (audioSource != null) { audioSource.Play(); }
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
    }

    private void DeathHandler()
    {
        playerIsAlive = false;
        if (audioSource != null) { audioSource.Stop(); }
        pauseMenu.SetActive(false);
        deathScreen.SetActive(true);
    }

    private void NewRoundBegin()
    {
        round.text = currentRound.ToString();
        SoundFXManager.instance.PlaySoundFXClip(Sounds.instance.RoundStart, transform, 1.5f);
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

    public void Resume() { StateManager.instance.UpdateGameState(GameStates.Resume); }
    public void MainMenu() { StateManager.instance.UpdateGameState(GameStates.MainMenu); }
    public void Restart() { StateManager.instance.UpdateGameState(GameStates.Restart); }
}
