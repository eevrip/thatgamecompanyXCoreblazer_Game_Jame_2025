using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LastCutSceneManager : MonoBehaviour
{
    #region Singleton
    public static LastCutSceneManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] private PlayerController player;
    [SerializeField] private AudioClip lastGlistering;
    [SerializeField] private ParticleSystem lastGlisteringParticleSystem; 
    [SerializeField] private bool isLastScene;
    [SerializeField] private bool stopChecking;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject uiTodisable;
    public bool IsLastScene { get { return isLastScene; } set { isLastScene = value; } }
   
    public void EnableLastCutScene()
    {
        
       
        StartCoroutine(AddNewLight());

    }
    void Update()
    {
        if (!stopChecking && isLastScene && Input.GetMouseButtonDown(1))
        {
            player.Burst();
            ToolTipManager.instance.HideToolTip();
            stopChecking = true;
            StartCoroutine(LastCutScene());
        }
    }
    IEnumerator AddNewLight()
    {

       
        yield return new WaitForSeconds(6f); //wait to fade out
       
        //Add white - disable all other colors
        player.AddToAvailableLights(6);
        yield return new WaitForSeconds(12f);
        //player.IsStatic = false; //dont make false - dont want to change color
        isLastScene = true;
        ToolTipManager.instance.ActivateInformation(1); //tell to burst


    }
    IEnumerator LastCutScene()
    {
        //Do the sequence of transitioning into credits

        SFXManager.instance.FadeSoundClipSecondary(2f, 0.5f, 0f);
        MusicManager.instance.FadeSoundClipMain(2f, 0.5f, 0f);
        MusicManager.instance.PlayClip(1, lastGlistering);
        yield return new WaitForSeconds(0.5f);
        EnvironmentManager.instance.LastCutScene(2, 8);
     
            uiTodisable.SetActive(false);
        yield return new WaitForSeconds(6.1f);

        StartCoroutine(LightPositionAnimation(10f, 7f, 0f));
       
        yield return new WaitForSeconds(5f);
        lastGlisteringParticleSystem.Play();
        yield return new WaitForSeconds(20f);


        player.gameObject.SetActive(false);



        EnvironmentManager.instance.ToCredits(credits);
       
    }


    IEnumerator LightPositionAnimation(float duration, float from, float to)
    {
        float timer = 0f;
        player.transform.position = new Vector3(0f, from, 0f);
       
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float y = Mathf.Lerp(from, to, timer / duration);
            player.transform.position = new Vector3(0f, y, 0f);
            yield return null;
        }
        player.transform.position = new Vector3(0f, to, 0f);
        yield return new WaitForSeconds(6f);
        player.PlayerAnimator.SetTrigger("dying");
    }
}
