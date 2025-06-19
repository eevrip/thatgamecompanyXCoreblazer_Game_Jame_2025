using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialHandler : MonoBehaviour
{
   private Material material;
    [SerializeField] private AudioClip glitteringClip;
    [SerializeField] private AudioClip putSound;
    private AudioSource audioSource;
    private void Start()
    {
       
            material = GetComponent<SpriteRenderer>().material;
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null )
            audioSource.clip = glitteringClip;
    }
    public void Dissolve()
    {
       
        StartCoroutine(Dissolving(4f, true));
        if (audioSource)
        {
            audioSource.Play();
            audioSource.clip = glitteringClip;
        }
            //SFXManager.instance.PlayClip(1, glitteringClip, 0.5f);
        }
    public void Dissolve(float duration)
    {
        
        StartCoroutine(Dissolving(duration, true));
        // SFXManager.instance.PlayClip(1, glitteringClip, 0.5f);
        if (audioSource)
        {
            audioSource.clip = glitteringClip;
            audioSource.Play();
        }
    }
    public void Dissolve(float duration, bool deactivate)
    {

        StartCoroutine(Dissolving(duration, deactivate));
        // SFXManager.instance.PlayClip(1, glitteringClip, 0.5f);
        if (audioSource)
        {
            audioSource.clip = glitteringClip;
            audioSource.Play();
        }
    }
    public void Appear(float duration)
    {
        StartCoroutine(Appearing(duration));
       // SFXManager.instance.PlayClip(1, glitteringClip, 0.5f);
         if (audioSource)
              audioSource.Play();
    }

    public void PlaySound()
    {
        SFXManager.instance.PlayClip(1, putSound, 0.5f);
    }

    public void EnableCollider()
    {
        if (this.gameObject.GetComponent<Collider2D>())
            this.gameObject.GetComponent<Collider2D>().enabled = true;
    }
    public void DisableCollider()
    {
        if (this.gameObject.GetComponent<Collider2D>())
            this.gameObject.GetComponent<Collider2D>().enabled = false;
    }
    IEnumerator Dissolving(float duration, bool deactivate)
    {
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        float timer = duration;

       
       
        ToolTipManager.instance.DeactivateInformation(0); 
        DisableCollider();
        material.SetFloat("_Fade", 1f);
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            material.SetFloat("_Fade", timer / duration);
            
            yield return null;
        }


        yield return new WaitForSeconds(0.1f); //for music

        EnvironmentManager.instance.UnblockButtonsChangeScenes();
       
       yield return new WaitForSeconds(5f);

       if(deactivate)
            gameObject.SetActive(false);
    }
    IEnumerator Appearing(float duration)
    {
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        float timer = 0f;

       


        material.SetFloat("_Fade", 0f);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            material.SetFloat("_Fade", timer / duration);
            yield return null;
        }
        EnvironmentManager.instance.UnblockButtonsChangeScenes();
    }
}
