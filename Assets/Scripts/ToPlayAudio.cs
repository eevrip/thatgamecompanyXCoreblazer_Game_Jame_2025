using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayAudio : MonoBehaviour
{
    [SerializeField] private AudioClip clipToPlay;
    // Start is called before the first frame update
    void Start()
    {
        SFXManager.instance.PlayClipLoop(1, clipToPlay, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
