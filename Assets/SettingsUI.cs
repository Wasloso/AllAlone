using UnityEngine;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider environmentSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        environmentSlider.onValueChanged.AddListener(SetEnvironmentVolume);
        InitializeSliderFromMixer(masterSlider, "MasterVolume");
        InitializeSliderFromMixer(musicSlider, "MusicVolume");
        InitializeSliderFromMixer(environmentSlider, "EnvironmentVolume");
    }

    public void SetMasterVolume(float volume)
    {

        mixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
    }

    public void SetEnvironmentVolume(float volume)
    {
        mixer.SetFloat("EnvironmentVolume", Mathf.Log10(Mathf.Clamp(volume, 0.001f, 1f)) * 20);
    }

    private void InitializeSliderFromMixer(Slider slider, string exposedParam)
    {
        if (mixer.GetFloat(exposedParam, out float dB))
        {
            float linear = Mathf.Pow(10f, dB / 20f);
            slider.SetValueWithoutNotify(linear);
        }
        else
        {
            slider.SetValueWithoutNotify(1f);
        }
    }
}


