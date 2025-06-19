using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Door : ToolTipEnvoker
{
   
    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject openDoor;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip squeakSound;
    [SerializeField] private List<AudioClip> lockDoorSounds;
    private AudioSource audioSource;
    private void Start()
    {
audioSource = GetComponent<AudioSource>();
    }
    public override void OnMouseEnter()
    {



        if (player.CurrentItem)
        {
            if (player.CurrentItem.type == Item.Type.Key)
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
   
    public void Interact()
    {
        if (player.CurrentItem)
        {

            if (player.CurrentItem.type == Item.Type.Key)
            {
                StartCoroutine(OpenDoor());
            }
            else
            {
                int randClip = Random.Range(0, lockDoorSounds.Count);
                //play sound lock door

                audioSource.volume = 0.5f;
                audioSource.clip = lockDoorSounds[randClip];
                audioSource.Play();
            }
        }
        else
        {
            int randClip = Random.Range(0, lockDoorSounds.Count);
            //play sound lock door
            
            audioSource.volume = 0.5f;
            audioSource.clip = lockDoorSounds[randClip];
            audioSource.Play();
        }
    }

    IEnumerator OpenDoor()
    {
            player.GiveItem();
        audioSource.volume = 0.4f;
        audioSource.clip = openDoorSound;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
       
        SFXManager.instance.PlayClip(1, squeakSound,0.5f);
       // audioSource.Play();
        yield return new WaitForSeconds(0.3f);

        openDoor.SetActive(true);
           
        gameObject.SetActive(false);
       
       
        
    }
}
