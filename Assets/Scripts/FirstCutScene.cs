using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirstCutScene : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    
    [SerializeField] private AudioClip soundClip;

    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip glisteringClip;
    [SerializeField] private SpriteRenderer lightSprite;
    [SerializeField] private SpriteRenderer burstSprite;
    [SerializeField] private Animator animator;
  

   
    private float alphaBurstingColor;

  
    void Start()
    {
      
       // Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;

      
    }
    private bool cutSceneFinished;
   public bool CutSceneFinished {  get { return cutSceneFinished; } }
    // Update is called once per frame

    void Update()
    {
        if (animator.GetFloat("IsStationary") != 1f && !cutSceneFinished)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            cutSceneFinished = true;


        }


        if (animator.GetFloat("IsStationary") != 1f && cutSceneFinished)
        {

            Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

            transform.position = new Vector3(mousePos.x, mousePos.y, 0f);

        }






       
        if (burstSprite.gameObject.activeSelf)
        {
            alphaBurstingColor = animator.GetFloat("BurstingAlpha");
            burstSprite.color = new Color(lightSprite.color.r, lightSprite.color.g, lightSprite.color.b, alphaBurstingColor);


        }
       
            if (CutsceneManager.instance.CanBurst && Input.GetMouseButtonDown(1))//Right click
            {
                SFXManager.instance.PlayClip(0,soundClip, 1f);
               
                animator.SetTrigger("burst");
                
            }

        }
    


    public void Explosion()
    {
        SFXManager.instance.PlayClip(0,explosionClip, 1f);
    }
    public void Glistering()
    {
        MusicManager.instance.PlayClip(0,glisteringClip);
    }

}




