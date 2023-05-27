using UnityEngine;

[CreateAssetMenu(fileName = "AudioOptions", menuName = "Audio/AudioOptions", order = 1)]
public class AudioOptions : ScriptableObject
{
    [Header("Pitch")]
    public bool VaryPitch;
    [HideInInspector] public RangedFloat PitchRange;
    [Range(-3f, 3f)] public float MinimumPitch = 0.75f;
    [Range(-3f, 3f)] public float MaximumPitch = 1.25f;

    [Header("Volume")]
    public bool VaryVolume;
    [HideInInspector] public RangedFloat VolumeRange;
    [Range(0f,1f)] public float MinimumVolume = 0.75f;
    [Range(0f,1f)] public float MaximumVolume = 1f;

    void OnEnable() => ValidateOptions();
    void OnValidate()
    {
        if (PitchRange == null || VolumeRange == null)
            ValidateOptions();
        
        if (PitchRange.Min != MinimumPitch || PitchRange.Max != MaximumPitch ||
            VolumeRange.Min != MinimumVolume || VolumeRange.Max != MaximumVolume)
            ValidateOptions();
    }

    void ValidateOptions()
    {
        PitchRange = new RangedFloat(MinimumPitch, MaximumPitch);
        VolumeRange = new RangedFloat(MinimumVolume, MaximumVolume);
    }
}
