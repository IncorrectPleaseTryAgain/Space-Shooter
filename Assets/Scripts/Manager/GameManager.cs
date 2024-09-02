using System;
using TMPro;
using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<GameObject> enemySpawners;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private List<AudioClip> backgroundMusic;
    [SerializeField] private AudioClip pauseMenuOpen;
    [SerializeField] private AudioClip pauseMenuClose;
    [SerializeField] private AudioClip roundBegin;
    [SerializeField] private AudioSource musicSource;

    private int currentRound;
    private int numEnemies;

    public TimeSpan timeElapsed { get; private set; }
    private Stopwatch stopwatch;
    private string time;

    private float musicTimer;
    private float currentMusicLength;
    private const int MIN_TIME_BETWEEN_MUSIC = 15;
    private const int MAX_TIME_BETWEEN_MUSIC = 15;

    private bool playerIsAlive;


    [SerializeField] private bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    private void Awake() { StateManager.OnGameStateChanged += OnGameStateChangedHandler; }
    private void OnDestroy() { StateManager.OnGameStateChanged -= OnGameStateChangedHandler; }

    private void Start() { GameStartHandler(); }

    void Update()
    {
        if (playerIsAlive)
        {
            // Pause & Resume
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused) { StateManager.instance.UpdateGameState(GameStates.GameResume); }
                else if (!IsPaused) { StateManager.instance.UpdateGameState(GameStates.GamePause); }
            }

            // Music Random Intervals
            if (musicTimer <= 0f)
            {
                PlayRandomMusic();
                musicTimer = UnityEngine.Random.Range(currentMusicLength + MIN_TIME_BETWEEN_MUSIC, currentMusicLength + MAX_TIME_BETWEEN_MUSIC);
            }
            musicTimer -= Time.deltaTime;

            // Timer
            TimeSpan timeSpan = stopwatch.Elapsed;
            time = (timeSpan.Hours > 0) ? String.Format("{0:00}:{1:00}:{2:00}.{3:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10)
                                         : String.Format("{0:00}:{1:00}.{2:00}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
            clockText.text = time;
        }
    }

    private void PlayRandomMusic()
    {
        int randomMusic = UnityEngine.Random.Range(0, backgroundMusic.Count);
        musicSource.PlayOneShot(backgroundMusic[randomMusic]);

        currentMusicLength = backgroundMusic[randomMusic].length;
    }

    private void OnGameStateChangedHandler(GameStates state)
    {
        switch (state)
        {
            case GameStates.GamePause:
                GamePauseHandler();
                break;
            case GameStates.GameResume:
                GameResumeHandler();
                break;
            case GameStates.NewRoundStart:
                NewRoundStartHandler();
                break;
            case GameStates.PlayerDeath:
                PlayerDeathHandler();
                break;
            case GameStates.EnemyDeath:
                EnemyDeathHandler();
                break;
        }
    }

    private void GameStartHandler()
    {
        currentRound = 0;
        numEnemies = 0;

        playerIsAlive = true;
        
        stopwatch = new Stopwatch();
        stopwatch.Start();
        
        musicTimer = UnityEngine.Random.Range(MIN_TIME_BETWEEN_MUSIC, MAX_TIME_BETWEEN_MUSIC);
        StateManager.instance.UpdateGameState(GameStates.NewRoundStart);
    }

    private void GamePauseHandler()
    {
        IsPaused = true;
        if (!pauseMenu.activeSelf) { pauseMenu.SetActive(true); }
        if (deathScreen.activeSelf) { deathScreen.SetActive(false); }
        AudioManager.instance.PlaySoundQueue(pauseMenuOpen);
    }
    private void GameResumeHandler()
    {
        IsPaused = false;
        if (pauseMenu.activeSelf) { pauseMenu.SetActive(false); }
        if (deathScreen.activeSelf) { deathScreen.SetActive(false); }
        AudioManager.instance.PlaySoundQueue(pauseMenuClose);
    }
    private void NewRoundStartHandler()
    {
        AudioManager.instance.PlaySoundQueue(roundBegin);

        currentRound++;
        roundText.text = currentRound.ToString();

        numEnemies = currentRound;
        for (int i = 0; i < numEnemies; i++)
        {
            int randomEnemyType = UnityEngine.Random.Range(0, enemyPrefabs.Count);
            int randomEnemySpawner = UnityEngine.Random.Range(0, enemySpawners.Count);

            enemySpawners[randomEnemySpawner].GetComponent<EnemySpawnerLogic>().SpawnEnemy(enemyPrefabs[randomEnemyType]);
        }
    }
    private void PlayerDeathHandler()
    {
        SaveManager.instance.SaveBestRound(currentRound, time);

        playerIsAlive = false;
        if (pauseMenu.activeSelf) { pauseMenu.SetActive(false); }
        if (!deathScreen.activeSelf) { deathScreen.SetActive(true); }
    }

    private void EnemyDeathHandler()
    {
        if (playerIsAlive)
        { 
            numEnemies--;
            if (numEnemies <= 0) { StateManager.instance.UpdateGameState(GameStates.NewRoundStart); }
        }
    }
}
