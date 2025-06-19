using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotesManager : MonoBehaviour
{
    public static NotesManager instance;
    public static bool gotNote;
    [SerializeField] private PlayerController player;
    [SerializeField] private List<GameObject> notes;
    [SerializeField] private GameObject theNote;
    [SerializeField] private AudioClip[] paperClips;
    [SerializeField] private GameObject notesUI;
    [SerializeField] private TextMeshProUGUI numberOfNotes;
    private bool isNoteActive;
    [SerializeField] private AudioSource audioSource;
    private int totalNotes;
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
      

    }
    private void Update()
    {
        if (!EnvironmentManager.isGivingSequence && !MenuManager.isGamePaused)
        {
            if (gotNote && Input.GetKeyDown(KeyCode.N))
            {


                if (!isNoteActive)
                {
                    OpenNote();
                }
                else
                {
                    CloseNote();
                }
            }
        }
    }
    public void UpdateListNotes(int note)
    {
        totalNotes++;
        numberOfNotes.text = "x" + totalNotes;
        notesUI.gameObject.SetActive(true);

        notes[note].SetActive(true);
        

    }
    public void OpenNote()
    {
      ToolTipManager.instance.HideToolTip();
        int randInt = Random.Range(0, paperClips.Length);
        audioSource.clip = paperClips[randInt];
        audioSource.Play();
        theNote.SetActive(true);
        isNoteActive = true;
        
        ToolTipManager.instance.ActivateInformation(4);  
        player.IsStatic = true;
        EnvironmentManager.instance.BlockButtonsChangeScenes();
        EnvironmentManager.instance.ToBlock();
    }
    public void CloseNote()
    {
       
        int randInt = Random.Range(0, paperClips.Length);
        audioSource.clip = paperClips[randInt];
        audioSource.Play();
        theNote.SetActive(false);
        isNoteActive = false;
        ToolTipManager.instance.DeactivateInformation(4);
        EnvironmentManager.instance.UnblockButtonsChangeScenes();
        EnvironmentManager.instance.Unblock(); 
        player.IsStatic = false;
    }
}
