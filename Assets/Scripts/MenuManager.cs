using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;
    
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
  
    public static bool isGamePaused = false;
    private bool pressedEsc;
    public Animator transitionLevel;

    public Animator transitionPauseMenu;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    public void PlayGame()
    {
        
        StartCoroutine(LoadingLevel(SceneManager.GetActiveScene().buildIndex + 1, 2f));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void ContinueGame()
    { //Time.timeScale = 1f;
        pressedEsc = false;
        if (transitionPauseMenu)
            transitionPauseMenu.SetTrigger("unload");
       
        Debug.Log("continue");
        StartCoroutine(UnloadPauseMenu());
        
    }
    public void PauseGame()
    {
        if (transitionPauseMenu)//there isnt  in first scene and credits 
        { if (!LastCutSceneManager.instance.IsLastScene && !CutsceneManager.isFirstCutScene)
            {
                if (!EnvironmentManager.isGivingSequence)
                {
                    pressedEsc = true;
                   transitionPauseMenu.SetTrigger("load");
                    Debug.Log("pause");
                   // pauseMenu.gameObject.SetActive(true);
                    StartCoroutine(LoadPauseMenu());
                }
                
            }
        }

    }

    public void MainMenu()
    {
        // ContinueGame();
        // SceneManager.LoadScene(0);
        isGamePaused = false;
        StartCoroutine(LoadingLevel(0,2f));
    }
    public void Credits()
    {

       
        StartCoroutine(LoadingLevel(2,2f));
    }
    public void LastSceneCredits()
    {


        StartCoroutine(LoadingLevel(2, 5f));
    }
    IEnumerator UnloadPauseMenu()
    {    
        Cursor.visible = false; 
        yield return new WaitForSeconds(0.5f);
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(0.4f);
        //Time.timeScale = 0f;
     
        Cursor.lockState = CursorLockMode.Confined;
       // if (pauseMenu)
         //   pauseMenu.gameObject.SetActive(false);
        isGamePaused = false;
       
       
    }
    IEnumerator LoadPauseMenu()
    {   
        yield return new WaitForSeconds(1f);
        
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(0.1f); 
        Cursor.visible = true;
        //Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
       
        isGamePaused = true;
    }
    IEnumerator LoadingLevel(int levelIndx, float duration)
    {


        if (transitionLevel)
            transitionLevel.SetTrigger("fadeIn");
        if (levelIndx == 1) //To the game
        {
            
        }
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(levelIndx);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pressedEsc)
            {

                PauseGame();
            }
            else
            {

                ContinueGame();
            }
        }
    }

}