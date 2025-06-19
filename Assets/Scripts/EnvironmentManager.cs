using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnvironmentManager : MonoBehaviour
{
    #region Singleton
    public static EnvironmentManager instance;

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
    public static bool isGivingSequence;
    [SerializeField] private PlayerController player;
    public Animator transitionLevel;
    public Animator transitionLevelWhite;
    [SerializeField] private GameObject Fireflies;
    [SerializeField] private GameObject blocker;
   [SerializeField] private List<GameObject> environments;
    [SerializeField] private List<Cat> cats;
    [SerializeField] private List<ColorSequence> colorSequence;
    [SerializeField] private List<GameObject> buttonsChangingScenes;
    [SerializeField] private AudioClip clipForest;
   public AudioClip clipKitchen;

    private int currentFrom;
    private bool isBlocking = false;
    public bool IsBlocking => isBlocking;
    public void TransitionFrom(int from)
    {
        currentFrom = from;
    }
    public void TransitionTo(int to)
    {
        
        StartCoroutine(LoadingEnvironment(currentFrom, to, transitionLevel));
    }
    public void LastCutScene(int from, int to)
    {
        currentFrom = from;
        
           
        
            StartCoroutine(TransitionToLastScene(currentFrom, to, transitionLevelWhite));
        
    }
    public void TransitionToSameScene(int currentScene, GameObject toActivate, GameObject toDeactivate)
    {
        StartCoroutine(ChangeCurrentEnvironment(currentScene, toActivate, toDeactivate));
    }
    public void ToBlock()
    {

        blocker.SetActive(true);
        isBlocking = true;
       
    }
    public void Unblock()
    {
        Debug.Log("Unblock img");
        blocker.SetActive(false);
        isBlocking= false;
    }
    public void BlockButtonsChangeScenes()
    {
       
        foreach (var button in buttonsChangingScenes)
            button.GetComponent<Collider2D>().enabled = false;
       // Debug.Log("block");
    }
    public void UnblockButtonsChangeScenes()
    {
        Debug.Log("Unblock button");
        foreach(var button in buttonsChangingScenes)
            button.GetComponent<Collider2D>().enabled = true;
      //  Debug.Log("unblock");
    }
    public void ToCredits(GameObject credits)
    {
        StartCoroutine(TransitionToCredits(credits, transitionLevel));
    }
    public void ResetCat(int cat, int toScene) {
        
        if (cat != 4)
        {//kitten
           if (cat == 2)//clock - (the cat number here is cat-1 )
            {
                if (cats[2].IsHappy)
                {

                    cats[2].gameObject.SetActive(false);
                }
                else
                {
                    cats[cat].gameObject.SetActive(true);
                    cats[cat].Reset();
                }
           }
            else
            {
                cats[cat].gameObject.SetActive(true);
                cats[cat].Reset();
            }
           
        }
        else
        {
            if (cats[0].IsHappy)//check if mum is happy
            {
                
                    cats[cat].gameObject.SetActive(true);
                    cats[cat].Reset();
                

            }
        }
    
    }


    IEnumerator ChangeCurrentEnvironment(int currentScene, GameObject toActivate, GameObject toDeactivate)
    {

       
        if (transitionLevel)
            transitionLevel.SetTrigger("fadeIn");

        yield return new WaitForSeconds(1.5f);
       // Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(0.5f);
       // ToolTipManager.instance.HideToolTip();

        if (toActivate) 
            toActivate.SetActive(true);
        if(toDeactivate)
            toDeactivate.SetActive(false);


        // player.DeactivateTrail();
       
        if (transitionLevel)
            transitionLevel.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.2f);
      //  player.ActivateTrail();
      //  Cursor.lockState = CursorLockMode.Confined;
      //  Cursor.visible = false;
    }
    

    IEnumerator TransitionToLastScene(int from, int to, Animator transition)
    {
       
        transition.gameObject.SetActive(true);
        transition.SetTrigger("fadeIn");

        PlayerController.IsRecordingSequence = false;
        player.ClearColorSequence();

        yield return new WaitForSeconds(1.5f); 
        
        environments[from].SetActive(false);
        ToolTipManager.instance.HideToolTip();

        Cursor.lockState = CursorLockMode.Locked;
       player.IsMovingMouse = true;
       
       
        yield return new WaitForSeconds(0.5f);


       
            Fireflies.SetActive(false);
       

        //was here
        environments[to].SetActive(true);
       

        yield return new WaitForSeconds(4f);
        transition.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.2f);
        player.ActivateTrail(); 
        player.IsStationary = true;
       
    }
    IEnumerator LoadingEnvironment(int from, int to, Animator transition)
    {


       // SFXManager.instance.FadeSoundClipSecondary(2f,0.5f, 0f);
            transition.SetTrigger("fadeIn");
      
        PlayerController.IsRecordingSequence = false;
        player.ClearColorSequence();
       
        yield return new WaitForSeconds(1.5f);
        environments[from].SetActive(false);
        ToolTipManager.instance.HideToolTip();

        Cursor.lockState = CursorLockMode.Locked; 
       
        yield return new WaitForSeconds(0.5f);
        

        for(int i=0; i<cats.Count; i++)
            ResetCat(i, to);
        foreach( ColorSequence seq in colorSequence)
            seq.Reset(); //unsubscribe from the delegate
        if(to == 5)
        {
            
            MusicManager.instance.PlayClipAtRandomTime(0,clipKitchen, 0.3f);
            Fireflies.SetActive(false);
        }
        if (from == 5)
        {
            
            MusicManager.instance.PlayClipAtRandomTime(0,clipForest,0.5f);
            Fireflies.SetActive(true);
        }
        //was here
        environments[to].SetActive(true);
        player.DeactivateTrail();
        
       
            transition.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.2f);
        player.ActivateTrail();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;  
        
    }


    IEnumerator TransitionToCredits(GameObject credits, Animator transition)
    {

        transition.gameObject.SetActive(true);
        transition.SetTrigger("fadeIn");
        yield return new WaitForSeconds(1.5f);
      

        yield return new WaitForSeconds(2f);  
        credits.SetActive(true);
        transition.SetTrigger("fadeOut");
        yield return new WaitForSeconds(3f);

       // transition.SetTrigger("fadeIn");
       // yield return new WaitForSeconds(1f);

        MenuManager.instance.LastSceneCredits();

    }



}
