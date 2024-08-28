using System;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public AudioClip PlayerHit;
    public AudioClip PlayerShoot;
    public AudioClip PlayerDeath;
    public AudioClip PlayerDeathRare;

    public AudioClip EnemyHit;
    public AudioClip EnemyDeath;

    public AudioClip RoundStart;

    public AudioClip Button;
    public AudioClip Start;
    
    
    public AudioClip PauseMenuOpen;
    public AudioClip PauseMenuClose;

    public AudioClip[] Game;
    public AudioClip MainMenu;
    public AudioClip Credits;
    public AudioClip Settings;
}