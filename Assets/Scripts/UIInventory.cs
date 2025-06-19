using System.Collections;

using TMPro;
using UnityEngine;

using UnityEngine.UI;


public class UIInventory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController player;
    [SerializeField] private Image currentItem;
    private Material material;
    [SerializeField] private CanvasGroup group;
   [SerializeField] private TextMeshProUGUI text;
    private float currentAlpha;
    void Start()
    {
        player.onInventoryCallback += UpdateUI;
        material = currentItem.material;
        material.SetFloat("_Fade",0f);
        currentAlpha = 0f;
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        if (player.CurrentItem != null)
        {

            
           //  StartCoroutine(Delay(1.5f, 0f, 1f, true));
            Debug.Log("change");
            currentItem.gameObject.SetActive(true);


            currentItem.sprite = player.CurrentItem.UiSprite;
            text.text = "x1";
            StartCoroutine(Fade(1f, 0f, 1f));

        }
        else
        {
           // StartCoroutine(Fade(1f, 1f, 0f));
              StartCoroutine(Delay(1f, 1f, 0f, false));
           // currentItem.gameObject.SetActive(false);
           // group.alpha = 0f;
        }
    }



    IEnumerator Delay(float duration, float from, float to, bool getOn)
    {
        
      /*  if (getOn)
        {
            
           
            currentItem.gameObject.SetActive(getOn);


            currentItem.sprite = player.CurrentItem.UiSprite;
            text.text = "x1"; 
           
            yield return new WaitForSeconds(2f);
        }*/
       
        StartCoroutine(Fade(duration, from, to));
        
        if (!getOn)
        { yield return new WaitForSeconds(2f);
           
            currentItem.gameObject.SetActive(getOn);
            
        }
      
    }
    IEnumerator Fade(float duration, float fadeFromAlpha, float fadeToAlpha)
    {
        float timer = 0;

       


        while (timer < duration)
        {
            timer += Time.deltaTime;
            
            group.alpha = Mathf.Lerp(fadeFromAlpha, fadeToAlpha, timer / duration);
           // currentAlpha = group.alpha;
            yield return null;
        }
        Debug.Log(player.CurrentItem);
    }
    
}
