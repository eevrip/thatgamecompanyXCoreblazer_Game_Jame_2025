using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{

    [SerializeField] private List<MaterialHandler> handlers = new List<MaterialHandler>();
    [SerializeField] private List<AudioClip> bounceClip = new List<AudioClip>();
    
    // Start is called before the first frame update
    public void EnableCollider()
    {
        if (this.gameObject.GetComponent<Collider2D>())
            this.gameObject.GetComponent<Collider2D>().enabled = true;
    }
    public void DisableCollider()
    {
        if (this.gameObject.GetComponent<Collider2D>())
            this.gameObject.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(DeactiveToy());
    }

    IEnumerator DeactiveToy()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }
    public void PlayBounce1()
    {
        SFXManager.instance.PlayClip(1, bounceClip[0], 0.5f);
    }
    public void PlayBounce2()
    {
        SFXManager.instance.PlayClip(1, bounceClip[1], 0.5f);
    }

}
