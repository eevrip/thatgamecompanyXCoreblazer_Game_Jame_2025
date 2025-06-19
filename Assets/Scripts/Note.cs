using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private int note;

    public void Interact()
    {
        NotesManager.gotNote = true;
        NotesManager.instance.UpdateListNotes(note);
        NotesManager.instance.OpenNote();
        ToolTipManager.instance.DeactivateInformation(0);
        ToolTipManager.instance.SetText("Interact");
        gameObject.SetActive(false);

    }

    public void EnableNote()
    {
        StartCoroutine(DelayEnableCollider());
    }

    IEnumerator DelayEnableCollider() {

        yield return new WaitForSeconds(4.1f);
        GetComponent<Collider2D>().enabled = true;

    }
}
