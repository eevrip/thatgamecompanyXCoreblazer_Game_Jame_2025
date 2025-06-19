using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorSequence : ToolTipEnvoker
{

   public PlayerController player;
  
    public List<int> sequenceColor = new List<int>();

    
    public virtual void OnEnable()
    {
        player.onSequenceCallback += IsCorrectSequence; 
        player.ClearColorSequence();
        PlayerController.LengthOfSequence = sequenceColor.Count;
        Debug.Log("Length " + PlayerController.LengthOfSequence);
        PlayerController.IsRecordingSequence = true;
        player.IsColorRegistering = true;

    }
    public void Reset()
    {
   
        player.onSequenceCallback -= IsCorrectSequence;
        Debug.Log(gameObject.name + "reset");
    }


   
   
    public virtual void Interact()
    {
       
    }
    public virtual void IsCorrectSequence()
    {

        player.ClearColorSequence();

    }
   

}