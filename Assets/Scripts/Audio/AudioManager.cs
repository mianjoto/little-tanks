using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public AudioOptions DefaultOptions;
    public static AudioManager Instance { get; private set; }
    
    void Awake()
    {
        Instance = this;
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }


    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        ApplyOptions(AudioManager.Instance.DefaultOptions);
        audioSource.PlayOneShot(clip);
    }

    public void PlaySound(AudioClip clip, AudioOptions clipOptions)
    {
        if (clip == null)
            return;

        if (clipOptions == null)
            clipOptions = DefaultOptions;

        ApplyOptions(clipOptions);
        audioSource.PlayOneShot(clip);
        ApplyOptions(DefaultOptions);
    }

    void ApplyOptions(AudioOptions clipOptions)
    {
        if (clipOptions.VaryPitch)
            audioSource.pitch = clipOptions.PitchRange.Value;
        if (clipOptions.VaryVolume)
            audioSource.volume = clipOptions.VolumeRange.Value;
    }
}
