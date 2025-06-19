using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipEnvoker : MonoBehaviour
{
    public string text;
    
    private void Start()
    {
    
    }
    public void SetText(string msg)
    {
        text = msg;
    }
    public virtual void OnMouseEnter()
    {
        
            
            if (text != "")
                ToolTipManager.instance.SetText(text);
            ToolTipManager.instance.ActivateInformation(0);// ShowToolTip();
        
    }
    public virtual void OnMouseExit()
    {
        // ToolTipManager.instance.HideToolTip();
        
            ToolTipManager.instance.DeactivateInformation(0);
        
    }
    
    
}
