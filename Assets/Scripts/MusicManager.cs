using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private AudioSource [] audioSourceMusic;
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
        audioSourceMusic = GetComponentsInChildren<AudioSource>();

    }
    public void PlayClip(int src, AudioClip clip)
    {

        audioSourceMusic[src].clip = clip;
        audioSourceMusic[src].Play();
    }
   
    public void PlayClipAtRandomTime(int src,AudioClip clip, float volume)
    {
        audioSourceMusic[src].Stop(); 
        audioSourceMusic[src].volume = volume;
        audioSourceMusic[src].clip = clip;
        float randomTime = Random.Range(0f,clip.length);
        
        audioSourceMusic[src].time = randomTime;
       
       
        audioSourceMusic[src].Play();
    }

    
    public void FadeSoundClipMain(float duration, float from, float to)
    {
        audioSourceMusic[0].volume = from;
       
        audioSourceMusic[0].Play();
        StartCoroutine(StartFadeSound(0, duration, from, to));
    }
    public IEnumerator StartFadeSound(int src,float duration, float fromVolume,float targetVolume)
    {
        float currentTime = 0;
       
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSourceMusic[src].volume = Mathf.Lerp(fromVolume, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
