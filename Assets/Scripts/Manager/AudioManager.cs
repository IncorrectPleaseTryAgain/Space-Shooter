using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Audio Mixer
    [SerializeField] AudioMixer audioMixer;
    public const string MIXER_MASTER_VOLUME_KEY = "Master Volume";
    public const string MIXER_MUSIC_VOLUME_KEY = "Music Volume";
    public const string MIXER_SFX_VOLUME_KEY = "SFX Volume";

    [SerializeField] private AudioSource ENEMY_AUDIO_SOURCE;
    [SerializeField] private AudioSource PLAYER_AUDIO_SOURCE;
    [SerializeField] private AudioSource PROJECTILE_AUDIO_SOURCE;
    [SerializeField] private AudioSource SOUND_QUEUE_AUDIO_SOURCE;

    // return clip length | float
    public float PlayEnemySFX(List<AudioClip> clips) { return Play(ENEMY_AUDIO_SOURCE, clips); }
    public float PlayPlayerSFX(List<AudioClip> clips) { return Play(PLAYER_AUDIO_SOURCE, clips); }
    public float PlayProjectileSFX(List<AudioClip> clips) { return Play(PROJECTILE_AUDIO_SOURCE, clips); }
    public void PlaySoundQueue(AudioClip clip) { Play(SOUND_QUEUE_AUDIO_SOURCE, clip); }

    private void Awake()
    {
        if(instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start() { LoadVolume(); }

    private void Play(AudioSource source, AudioClip clip){ source.PlayOneShot(clip); }
    private float Play(AudioSource source, List<AudioClip> clips)
    {
        int rand = UnityEngine.Random.Range(0, clips.Count);
        source.PlayOneShot(clips[rand]);
        return clips[rand].length;
    }


    // Load Audio Volume | Audio Saved In VolumeSettings.cs
    private void LoadVolume()
    {
        float volume;

        volume = PlayerPrefs.GetFloat(MIXER_MASTER_VOLUME_KEY, 0.5f);
        audioMixer.SetFloat(VolumeSettings.MIXER_MASTER_VOLUME, Mathf.Log10(volume) * 20f);

        volume = PlayerPrefs.GetFloat(MIXER_MUSIC_VOLUME_KEY, 0.5f);
        audioMixer.SetFloat(VolumeSettings.MIXER_MUSIC_VOLUME, Mathf.Log10(volume) * 20f);
        
        volume = PlayerPrefs.GetFloat(MIXER_SFX_VOLUME_KEY, 0.5f);
        audioMixer.SetFloat(VolumeSettings.MIXER_SFX_VOLUME, Mathf.Log10(volume) * 20f);

    }
}
