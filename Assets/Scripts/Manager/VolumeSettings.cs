using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    public const string MIXER_MASTER_VOLUME = "Master Volume";
    public const string MIXER_MUSIC_VOLUME = "Music Volume";
    public const string MIXER_SFX_VOLUME = "SFX Volume";

    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMaserVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void OnDisable() { SaveVolumeSettings(); }

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_MASTER_VOLUME_KEY, 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_MUSIC_VOLUME_KEY, 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MIXER_SFX_VOLUME_KEY, 0.5f);
    }


    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MASTER_VOLUME_KEY, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC_VOLUME_KEY, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_SFX_VOLUME_KEY, sfxVolumeSlider.value);
    }

    private void SetMaserVolume(float value) { audioMixer.SetFloat("Master Volume", Mathf.Log10(value) * 20f); }
    private void SetMusicVolume(float value) { audioMixer.SetFloat("Music Volume", Mathf.Log10(value) * 20f); }
    private void SetSFXVolume(float value) { audioMixer.SetFloat("SFX Volume", Mathf.Log10(value) * 20f); }
}
