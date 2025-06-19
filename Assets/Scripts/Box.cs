using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Vines vines;
    private Collider2D col;
    void Start()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        vines.onVinesCallback += UpdateBox;
    }

    // Update is called once per frame
    void UpdateBox()
    {
        StartCoroutine(Delay(5.1f));
            
    }


    IEnumerator Delay(float duration)
    {

        yield return new WaitForSeconds(duration);

        
       col.enabled = true;
            
    }
}
