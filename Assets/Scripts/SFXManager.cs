using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource[] audioSourceSfx;
    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        audioSourceSfx = GetComponentsInChildren<AudioSource>();

    }
    public void PlayClip(int audioSource, AudioClip clip, float volume)
    {
        audioSourceSfx[audioSource].loop = false;
        audioSourceSfx[audioSource].volume = volume;
        audioSourceSfx[audioSource].clip = clip;
        audioSourceSfx[audioSource].Play();
    }
    public void PlayClipLoop(int audioSource, AudioClip clip, float volume)
    {
        audioSourceSfx[audioSource].loop = true;
        audioSourceSfx[audioSource].volume = volume;
        audioSourceSfx[audioSource].clip = clip;
        audioSourceSfx[audioSource].Play();
    }
    public void PauseClip(int audioSource)
    {

        audioSourceSfx[audioSource].Pause();
        audioSourceSfx[audioSource].clip = null;
    }

    public void FadeSoundClipSecondary(float duration, float from, float to)
    {
        audioSourceSfx[3].volume = from;

        audioSourceSfx[3].Play();
        StartCoroutine(StartFadeSound(1, duration, from, to));
    }
    public IEnumerator StartFadeSound(int src, float duration, float fromVolume, float targetVolume)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSourceSfx[src].volume = Mathf.Lerp(fromVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
