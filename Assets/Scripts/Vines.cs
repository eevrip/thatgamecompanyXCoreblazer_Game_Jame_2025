using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vines : ColorSequence
{
    // Start is called before the first frame update

    [SerializeField] private GameObject toActivate;
    [SerializeField] private GameObject releaseItem;
    [SerializeField] private bool isAbandonedVines;
    private MaterialHandler handler;
    public delegate void OnVinesDisappear();
    public OnVinesDisappear onVinesCallback;
    [SerializeField] Animator animator;
    [SerializeField] private AudioClip[] vines = new AudioClip[2];
    [SerializeField] private AudioClip xylophoneAll;
    private bool isComplete = false;
    public bool isCutSceneFinished = true;
    public bool tutorial;
    void Start()
    {
        handler = GetComponent<MaterialHandler>();

    }
    public override void OnEnable()
    {
        base.OnEnable();
        if (isComplete)
            player.IsColorRegistering = false;
        if (!isCutSceneFinished)
        {
            player.IsColorRegistering = false;
            //StartCoroutine(EnableVines(6f));
        }
    }

    public override void OnMouseEnter()
    {
        if (!tutorial)
            base.OnMouseEnter();
        else
        {
            if (isCutSceneFinished)
            {


                Debug.Log(gameObject.name);
                ToolTipManager.instance.ActivateInformation(1);
                player.IsColorRegistering = true;

            }

        }
    }
    public override void OnMouseExit()
    {
        if (!tutorial)
            base.OnMouseExit();
        
    }
    public override void IsCorrectSequence()
    {

        if (!isComplete)
        {
            List<int> givenSequence = player.GetColorSequence();

            for (int i = 0; i < givenSequence.Count; i++)
            {

                if (sequenceColor[i] != givenSequence[i])
                {
                    if (animator)
                        animator.SetTrigger("bursting");
                    base.IsCorrectSequence();
                    return;
                }

            }
            if (tutorial)
            {
                ToolTipManager.instance.DeactivateInformation(1);
            }

            if (isAbandonedVines)
            {
                StartCoroutine(GivingSequence());
               
            }
            else
                StartCoroutine(Delay(0.1f));
        }

    }

    public void PlayClip()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        int randClip = Random.Range(0, vines.Length);
        audioSource.clip = vines[randClip];
        audioSource.Play();
    }


    IEnumerator Delay(float duration)
    {

        yield return new WaitForSeconds(duration);
        if (onVinesCallback != null)
        {
            onVinesCallback.Invoke();
        }
        player.onSequenceCallback -= IsCorrectSequence;
        handler.Dissolve(5f);



        isComplete = true;

    }

    public void EnableVine(float duration)
    {
       
        StartCoroutine(EnableVines(duration));
    }
    IEnumerator EnableVines(float duration)
    {
        yield return new WaitForSeconds(duration);
        handler.EnableCollider();
        isCutSceneFinished = true;
    }







    IEnumerator GivingSequence()
    {

        player.IsStatic = true;
        player.IsColorRegistering = false;
        EnvironmentManager.isGivingSequence = true;
        SFXManager.instance.PlayClip(1, xylophoneAll,0.5f);
        yield return new WaitForSeconds(3f);
        EnvironmentManager.instance.ToBlock();
        handler.Dissolve(10f, false);

       

        



        yield return new WaitForSeconds(10.2f);
       
        EnvironmentManager.instance.BlockButtonsChangeScenes(); //to ensure blocking
 
        EnvironmentManager.instance.TransitionToSameScene(0, toActivate, null); //activate cats
        LastCutSceneManager.instance.EnableLastCutScene();
        isComplete = true;



    }
}
