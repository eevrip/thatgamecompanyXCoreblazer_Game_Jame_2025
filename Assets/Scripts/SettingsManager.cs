using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private static float masterVolume = 1f;
    private static float musicVolume = 1f;
    private static float sfxVolume = 1f;
    private static float ambientVolume = 1f;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambientSlider;
    // Start is called before the first frame update
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);

    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
    }
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }
    public void SetAmbientVolume(float volume)
    {
        ambientVolume = volume;
        audioMixer.SetFloat("AmbientVolume", Mathf.Log10(volume) * 20f);
    }
    void Start()
    {

      //  masterSlider.value = masterVolume;


       // musicSlider.value = musicVolume;


        sfxSlider.value = sfxVolume;

       // ambientSlider.value = ambientVolume;
    }
}
