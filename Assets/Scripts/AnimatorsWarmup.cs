using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorsWarmup : MonoBehaviour
{
    public Animator[] uiAnimators;

    void Start()
    {
        foreach (var animator in uiAnimators)
        {
            animator.Play("Base Layer.idle", 0, 1f); // Play and immediately set to end
            animator.Update(0); // Forces the Animator to update
        }
    }
}
