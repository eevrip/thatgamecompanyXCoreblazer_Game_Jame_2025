using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

public class Cat : ToolTipEnvoker
{
    [SerializeField] private int cat;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator thoughtsAnimator;
    [SerializeField] private Item wantedItem;

    [SerializeField] private GameObject givenItem;
    [SerializeField] private GameObject toActivate;
    [SerializeField] private GameObject toDeactivate;
    [SerializeField] private Color eyeColor;
    [SerializeField] private AudioClip purring;
    [SerializeField] private AudioClip meowing;
    [SerializeField] private PlayerController player;
    [SerializeField] private AudioSource source;
    [SerializeField] private SpriteRenderer eyes;
    [SerializeField] private GameObject thoughts;
    [SerializeField] private SpriteRenderer spriteWantedItem;
    [SerializeField] private GameObject spriteNotWant;

    private State state;
    private bool completePurring = true;
    private bool showingThoughts = false;
    private bool completeGiving = true;
    private bool isHappy = false;
    private bool onCat = false;
    public bool IsHappy { get { return isHappy; } }
    public enum State
    {
        Idle,

        Purring,
        Happy
    }
    private void Start()
    {


    }
    public void Reset()
    {
        Debug.Log("Reset cat " + cat);
            StopSound();
            player.onBurstingCallback -= TriggerPurring;
            completePurring = true;
            onCat = false;
            ResetThoughts();
            eyes.color = eyeColor;
       


    }
    private void OnEnable()
    {
        player.onBurstingCallback += TriggerPurring;
        if (isHappy)
        {
            state = State.Happy; //might need to be happy


            if (cat == 5) //kitten
            {
                animator.SetLayerWeight(1, 0);//eyes layer 1
                animator.SetLayerWeight(2, 0);//purring layer 2
                animator.SetLayerWeight(3, 1);//playing layer 3
            }
            else
            {
                animator.SetLayerWeight(1, 0);//eyes layer 1
                animator.SetLayerWeight(2, 1);//happy layer 2}


            }
        }
        else
        {
            state = State.Idle;
            animator.SetLayerWeight(2, 0);//happy layer 2
            animator.SetLayerWeight(1, 1);//eyes layer 1
            if (cat == 5) //kitten
            {
                
                animator.SetLayerWeight(3, 0);//playing layer 3
            }

        }
    }
    public override void OnMouseEnter()
    {
        if (cat != 3)
            player.IsColorRegistering = false;
        Debug.Log("register" + player.IsColorRegistering);
        if (state == State.Idle)
        {
            if (player.CurrentItem)
            {
                // ToolTipManager.instance.SetText("Give " + player.CurrentItem.itemName);
                text = "Give " + player.CurrentItem.itemName;
                base.OnMouseEnter();
            }

            ToolTipManager.instance.ActivateInformation(3);
        }
        if (completeGiving)
            ToolTipManager.instance.ActivateInformation(1);
    }
    public override void OnMouseExit()
    {
        onCat = false;
        Debug.Log("Exit" + player.IsColorRegistering);
        if (cat != 3)
        {
            player.IsColorRegistering = true;

        }
        Debug.Log("Exit later" + player.IsColorRegistering);
        base.OnMouseExit();
        ToolTipManager.instance.DeactivateInformation(1);
        ToolTipManager.instance.DeactivateInformation(3);
        ToolTipManager.instance.SetText("Interact");

    }
    public void OnMouseOver()
    {
        onCat = true;
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return; // Mouse is over a UI element - skip
        /* if (player.IsBursting && completePurring){


             StartCoroutine(Purring(state, 1.5f));

         }*/
        if (completePurring && Input.GetKeyDown(KeyCode.H))//Hint
        {
            TriggerThoughts(wantedItem, true);
        }
        //Check if not activateinformation from tooltipmanager to activate
        //ToolTipManager.instance.ActivateInformation(1);
        // ToolTipManager.instance.ActivateInformation(3);
    }
    public void Interact()
    {
        Debug.Log("interacting");

        if (completePurring && player.CurrentItem)
            GetItem(player.CurrentItem);
    }
    public void GetItem(Item item)
    {

        if (item.type == this.wantedItem.type)
        {
            state = State.Happy;
            isHappy = true;
            player.GiveItem();

            StartCoroutine(GivingSequence());
        }
        else
        {
            TriggerThoughts(item, false);
        }
    }
    public void GiveItem()
    {
        if (givenItem)
        {
            if (givenItem.CompareTag("ClockHands"))
            {
                givenItem.GetComponent<MaterialHandler>().EnableCollider();
            }
            else
            {
                givenItem.SetActive(true);//key and toy okay
                givenItem.GetComponent<ItemAnimatorHandler>().ToGiveItem();
            }
        }

    }


    public void TriggerThoughts(Item item, bool want)
    {
        if ( !isHappy &&!showingThoughts)
        {
            spriteWantedItem.sprite = item.UiSprite;

            // spriteNotWant.SetActive(!want);
            StartCoroutine(ThoughtsFade(want));
        }

    }

    public void TriggerPurring()
    {
        if (completePurring && onCat)
        {
            StartCoroutine(Purring(state, 1.5f));
        }
    }
    public void SetSound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
    public void StopSound()
    {

        source.Stop();
        //source.clip = null;
    }
    public void ResetThoughts()
    {
        showingThoughts = false;
        thoughts.SetActive(false);
        spriteWantedItem.gameObject.SetActive(false);
        spriteNotWant.SetActive(false);
        thoughts.GetComponent<SpriteRenderer>().sprite = null;
    }



    IEnumerator ThoughtsFade(bool want)
    {
        showingThoughts = true;
        thoughts.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        spriteWantedItem.gameObject.SetActive(true);
        spriteNotWant.SetActive(!want);
        yield return new WaitForSeconds(5f);
        thoughtsAnimator.SetTrigger("disappear");
        spriteWantedItem.gameObject.SetActive(false);
        spriteNotWant.SetActive(false);

        yield return new WaitForSeconds(0.4f);
        thoughts.SetActive(false);
        showingThoughts = false;
    }

    IEnumerator GivingSequence()
    {
        Debug.Log("Running GivingSequence on: " + gameObject.name + ", cat = " + cat);
        EnvironmentManager.isGivingSequence = true;
        player.IsStatic = true;
        player.IsColorRegistering = false;
        ToolTipManager.instance.HideToolTip();


        completeGiving = false;
        EnvironmentManager.instance.ToBlock();
        // EnvironmentManager.instance.TransitionToSameScene(0, toActivate, null);
        if (toDeactivate)
        {
            MaterialHandler matHandler = toDeactivate.GetComponent<MaterialHandler>();
            if (matHandler)
                matHandler.Dissolve(3f);
            yield return new WaitForSeconds(2f);
        }
        if (toActivate)
        {
            MaterialHandler matHandler = toActivate.GetComponent<MaterialHandler>();
            if (matHandler)
                matHandler.Appear(3f);
            yield return new WaitForSeconds(2f);
        }

        //  yield return new WaitForSeconds(2f);

        StartCoroutine(Purring(State.Happy, 1.5f));
        yield return new WaitForSeconds(1f);
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        yield return new WaitForSeconds(5f);
        player.AddToAvailableLights(cat);
        yield return new WaitForSeconds(12f);



        if (cat == 3)
        {

            EnvironmentManager.instance.TransitionToSameScene(0, null, null);
            yield return new WaitForSeconds(1.6f);
            player.onBurstingCallback -= TriggerPurring;
            GiveItem();
            //yield return new WaitForSeconds(0.5f);
            player.IsStatic = false;//because it deactivates later
            player.IsColorRegistering = false;

            EnvironmentManager.instance.Unblock();
            completeGiving = true;
            isHappy = true;
            Debug.Log("giving - registering " + player.IsColorRegistering);
            ToolTipManager.instance.ActivateInformation(2);//to change color
            EnvironmentManager.instance.UnblockButtonsChangeScenes();
            EnvironmentManager.isGivingSequence = false;
            this.gameObject.SetActive(false);
           
            yield break;
        }
        else
        {
            GiveItem();
            if (givenItem)
            {
                if (givenItem.CompareTag("Toy"))
                    yield return new WaitForSeconds(2.2f);
                else if (givenItem.CompareTag("Key"))
                    yield return new WaitForSeconds(1.2f);
            }
            EnvironmentManager.instance.Unblock();
            completeGiving = true;
            isHappy = true;

            ToolTipManager.instance.ActivateInformation(2);//to change color
            EnvironmentManager.instance.UnblockButtonsChangeScenes();
            player.IsStatic = false;
            player.IsColorRegistering = true;
            EnvironmentManager.isGivingSequence = false;
        }


        //Debug.Log(cat + " giving - registering after" + player.IsColorRegistering);

    }








    IEnumerator Purring(State nextState, float duration)
    {
        completePurring = false;
        float timer = 0;
        float fadeDuration = duration; // Just to be extra clear
        float startVolume = 0.6f;
        float endVolume = 0f;
        float fadeTimer = 0f;
        SetSound(purring);
        //  animator.SetTrigger("purring");
        animator.SetLayerWeight(2, 1);//happy layer 2
        animator.SetLayerWeight(1, 0);//eyes layer 1
        if(cat == 5)
            animator.SetLayerWeight(3, 0);//playing layer 3
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            source.volume = Mathf.Lerp(0f, 0.6f, t);


            yield return null;
        }
        state = State.Purring;

        yield return new WaitForSeconds(4f);



        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(fadeTimer / fadeDuration);
            source.volume = Mathf.Lerp(startVolume, endVolume, t);

            yield return null;
        }



        if (source.volume == 0f)
        {
            Debug.Log("stop");
            StopSound();
        }


        state = nextState;
        if (state == State.Idle)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetLayerWeight(2, 0);
        }
        if (state == State.Happy && cat == 5)
        {
            animator.SetLayerWeight(1, 0);//eyes layer 1
            animator.SetLayerWeight(2, 0);//purring layer 2
            animator.SetLayerWeight(3, 1);//playing layer 3
        }
        Debug.Log("state - " + state + " cat " + cat);
        completePurring = true;

    }
}
