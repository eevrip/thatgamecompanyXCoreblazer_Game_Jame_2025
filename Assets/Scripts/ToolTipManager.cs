using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> listOfInformation;
    [SerializeField] private TextMeshProUGUI text;
    
   
    #region Singleton
    public static ToolTipManager instance;
   
    void Awake()
    {

        instance = this;
        
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
       
       // gameObject.SetActive(false);
        foreach (GameObject go in listOfInformation)
        {
            go.SetActive(false);
        }
        
       
    }
    private Vector3 currentItemPos;


    public void SetText(string msg)
    {
        text.text = msg;
    }
    public void ActivateInformation(int idx)
    {
        if (!MenuManager.isGamePaused)
        {
            if (!EnvironmentManager.instance.IsBlocking)
            {
                listOfInformation[idx].SetActive(true);
            }
            else if (LastCutSceneManager.instance.IsLastScene && idx == 1)
            {
                listOfInformation[idx].SetActive(true);
            }
        }
    }
    public void DeactivateInformation(int idx)
    {
       
        SetText("Interact");
        
            listOfInformation[idx].SetActive(false);
        
    }
    public void ShowToolTip()
    {
        if (!EnvironmentManager.instance.IsBlocking)
        {
            Debug.Log("show all");


            gameObject.SetActive(true);
        }
    }

    public void HideToolTip()
    {
        // gameObject.SetActive(false);
        Debug.Log("hide all");
        foreach (GameObject go in listOfInformation)
            go.SetActive(false);
       
       SetText("Interact");
    }
   
}
