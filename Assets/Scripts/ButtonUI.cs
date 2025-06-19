using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private bool isSlider;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip hoverSfx;
    [SerializeField] private AudioClip pressedSfx;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectColor;
    private Image currentImage;
    private SFXManager sfxManager;
    [SerializeField] private int layer;

    public void Start()
    {
        currentImage = GetComponent<Image>();
        sfxManager = SFXManager.instance;
        if(isSlider )
        {
            animator.SetLayerWeight(1, 1);//enable this layer
        }
        else
        {
            animator.SetLayerWeight(1, 0);//disable this layer
        }
    }
    public void SelectButton()
    {
       sfxManager.PlayClip(2, hoverSfx, 0.5f);
        
        if (animator)
        {
            animator.SetBool("select", true);
            animator.SetLayerWeight(layer, 1);
        }
        if (currentImage) 
            currentImage.color = selectColor;
    }
    public void DeselectButton()
    {
        
        if (animator)
        {
            animator.SetBool("select", false);
            animator.SetLayerWeight(layer, 0);
        }
        if (currentImage)
            currentImage.color = defaultColor;
    }
    public void PressedButton()
    {
        sfxManager.PlayClip(2, pressedSfx, 0.5f);
        if (animator)
            animator.SetBool("pressed", true);
    
}
    public void UnpressedButton()
    {
        if (animator)
            animator.SetBool("pressed", false);

    }

    
    
}
