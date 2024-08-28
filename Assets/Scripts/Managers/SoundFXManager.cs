//Template: https://www.youtube.com/watch?v=DU7cgVsU2rM

using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceObj;

    public static SoundFXManager instance;

    private void Awake() { if(instance == null) { instance = this; } }

    public void PlaySoundFXClip(AudioClip clip, Transform spawnTransform, float volume, bool loop = false)
    {
        AudioSource audioSource = Instantiate(audioSourceObj, spawnTransform.position, Quaternion.identity);

        audioSource.clip = clip;

        audioSource.volume = volume;

        audioSource.loop = loop;
        audioSource.Play();

        if (!loop)
        {
            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }

    public AudioSource PlayRandomSoundFXClip(AudioClip[] clips, Transform spawnTransform, float volume)
    {
        int rand = UnityEngine.Random.Range(0, clips.Length-1);

        AudioSource audioSource = Instantiate(audioSourceObj, spawnTransform.position, Quaternion.identity);

        audioSource.clip = clips[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, clipLength);

        //Debug.Log("Index: " + rand);
        //Debug.Log("Length: " + clipLength + "s");
        return audioSource;
    }
}
