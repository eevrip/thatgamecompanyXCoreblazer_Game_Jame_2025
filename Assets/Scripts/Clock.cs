using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Clock : ColorSequence
{


    [SerializeField] private GameObject smallHand;
    [SerializeField] private GameObject bigHand;
    [SerializeField] private AudioClip placeHand;
    [SerializeField] private AudioClip rotateHands;
    [SerializeField] private AudioClip changeHour;
    [SerializeField] private AudioClip ticktock;
    [SerializeField] private AudioClip[] gears = new AudioClip[2];
    // Start is called before the first frame update

    [SerializeField] private float smallHandAngle;
    [SerializeField] private float bigHandAngle;
    private bool isClockReseting = false;
    private bool isSmallHandGiven = false;
    [SerializeField] private bool isComplete;
    public bool IsComplete => isComplete;
    public delegate void OnClockComplete();
    public OnClockComplete onClockcallback;
    public override void OnEnable()
    {
        base.OnEnable();
        if(!isSmallHandGiven)
            player.IsColorRegistering = false;
      
        smallHand.transform.rotation = Quaternion.Euler(0f, 0f, smallHandAngle);
        bigHand.transform.rotation = Quaternion.Euler(0f, 0f, bigHandAngle);
    }
    
    public override void OnMouseEnter()
    {



        if (player.CurrentItem)
        {
            if (player.CurrentItem.type == Item.Type.ClockHands)
            {
                // ToolTipManager.instance.SetText("Give " + player.CurrentItem.itemName);
                text = "Use " + player.CurrentItem.itemName;

            }
        }
        base.OnMouseEnter();

    }
    public override void OnMouseExit()
    {

        base.OnMouseExit();

        ToolTipManager.instance.SetText("Interact");

    }
    private float currentAngle;
    public override void Interact()
    {
        if (player.CurrentItem)
        {

            if (player.CurrentItem.type == Item.Type.ClockHands)
            {
                StartCoroutine(PlaceHand());
            }
            else
            {
                PlayClip();
            }
        }
        else
        {

            PlayClip();


        }
    }

    public override void IsCorrectSequence()
    {


        if (isSmallHandGiven && !isClockReseting && !isComplete)
        {
            List<int> givenSequence = player.GetColorSequence();
            for (int i = 0; i < givenSequence.Count; i++)
            {

                if (sequenceColor[i] != givenSequence[i])
                {
                    StartCoroutine(ResetClock());
                    base.IsCorrectSequence();
                    return;
                }

            }
            if (onClockcallback != null)
                onClockcallback.Invoke();
            player.ClearColorSequence();
            StartCoroutine(SetCorrectHour());
            player.onSequenceCallback -= IsCorrectSequence;

        }

    }

    public void PlayClip()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        int randClip = Random.Range(0, gears.Length);
        audioSource.clip = gears[randClip];
        audioSource.Play();
    }
    IEnumerator PlaceHand()
    {
        player.GiveItem();
        this.gameObject.GetComponent<Collider2D>().enabled = false;


        // smallHand.SetActive(true);
        MaterialHandler matHandler = smallHand.GetComponent<MaterialHandler>();
        if (matHandler)
            matHandler.Appear(3f);
        yield return new WaitForSeconds(3f);


        isSmallHandGiven = true;
       player.IsColorRegistering = true;



    }

    IEnumerator SetCorrectHour()
    {
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        SFXManager.instance.PlayClipLoop(1, rotateHands, 0.5f);
        float angleSmall = -90f;
        float angleBig = -3 * 360f;





        StartCoroutine(RotateOverTime(smallHand.transform, smallHandAngle, angleSmall, 3f, false));
        StartCoroutine(RotateOverTime(bigHand.transform, bigHandAngle, angleBig, 3f, true));
        smallHandAngle = smallHandAngle + angleSmall;
        bigHandAngle = bigHandAngle + angleBig;

        yield return new WaitForSeconds(3f);
        //SFXManager.instance.PauseClip(1);
        SFXManager.instance.PlayClip(1, changeHour, 0.6f);

        isClockReseting = false;
        player.IsColorRegistering = false; //correct sequence
        isComplete = true;
        EnvironmentManager.instance.UnblockButtonsChangeScenes();
    }

    IEnumerator ResetClock()
    {
        player.IsColorRegistering = false;
        isClockReseting = true;
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        SFXManager.instance.PlayClipLoop(1, rotateHands, 0.5f);
        float angleSmall = -90f;
        float angleBig = -3 * 360f;





        StartCoroutine(RotateOverTime(smallHand.transform, smallHandAngle, angleSmall, 2f, false));
        StartCoroutine(RotateOverTime(bigHand.transform, bigHandAngle, angleBig, 2f, true));
        smallHandAngle = smallHandAngle + angleSmall;
        bigHandAngle = bigHandAngle + angleBig;

        yield return new WaitForSeconds(2.1f);



        angleSmall = 90f;
        angleBig = 3 * 360f;





        StartCoroutine(RotateOverTime(smallHand.transform, smallHandAngle, angleSmall, 2f, false));
        StartCoroutine(RotateOverTime(bigHand.transform, bigHandAngle, angleBig, 2f, true));
        smallHandAngle = smallHandAngle + angleSmall;
        bigHandAngle = bigHandAngle + angleBig;
        yield return new WaitForSeconds(2f);
        SFXManager.instance.PauseClip(1);

        isClockReseting = false;
        player.IsColorRegistering = true;

        EnvironmentManager.instance.UnblockButtonsChangeScenes();

    }




    IEnumerator RotateOverTime(Transform handRotation, float startAngle, float angle, float duration, bool correct)
    {

        float elapsed = 0f;

        // Quaternion startRot = handRotation.rotation;
        // Quaternion endRot = startRot * Quaternion.Euler(0, 0, angle);

        float endAngle = startAngle + angle;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            


            float currentZ = Mathf.Lerp(startAngle, endAngle, t);
            handRotation.rotation = Quaternion.Euler(0, 0, currentZ);

            yield return null;
        }

        handRotation.rotation = Quaternion.Euler(0, 0, endAngle);



        isClockReseting = false;
        player.IsColorRegistering = true;


    }








}