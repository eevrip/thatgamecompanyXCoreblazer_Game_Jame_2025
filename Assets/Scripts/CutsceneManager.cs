using System.Collections;

using TMPro;

using UnityEngine;


public class CutsceneManager : MonoBehaviour
{
    #region 
    public static CutsceneManager instance;
    void Awake()
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
    [SerializeField] private PlayerController player;
    [SerializeField] private ParticleSystem birthingEffect;
    [SerializeField] private AudioClip magicalGlistering;
   
    [SerializeField] private SpriteRenderer blackBackground;
    [SerializeField] private SpriteRenderer vineRenderer;
    [SerializeField] private SpriteRenderer foregroundRenderer;
    [SerializeField] private Vines vine;
    [SerializeField] private ParticleSystem[] fireFlies;
    [SerializeField] private TextMeshProUGUI textStart;
    
    [SerializeField] private CanvasGroup groupMove;
    [SerializeField] private CanvasGroup groupBurst;

    // Update is called once per frame
    private bool hasPressedKey;
    private bool isCutSceneDone;
    private bool isInfoMoveDone;
    private bool canBurst;

    public static bool isFirstCutScene = true;
    public bool CanBurst {  get { return canBurst; } }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
       
            if (Input.anyKey && !hasPressedKey)
            {
                Debug.Log("A key or mouse click has been detected");
                birthingEffect.Play();
                MusicManager.instance.PlayClip(1, magicalGlistering);
                StartCoroutine(Birthing());
                hasPressedKey = true;
            }

            if (isCutSceneDone)
            {
                StartCoroutine(TextInfoMove());
                isCutSceneDone = false;
            }
            if (isInfoMoveDone)
            {
                canBurst = true;
                if (Input.GetMouseButtonDown(1))
                {

                    StartCoroutine(TextInfoBurst());
                    vine.EnableVine(3f);
                    StartCoroutine(TransitionBlackScreen(4f, 1f, 0f));

                    isInfoMoveDone = false;
                }
            }
           
    }
    IEnumerator Birthing()
    {
        StartCoroutine(FadeText(1f, 1f,0f, textStart));
        yield return new WaitForSeconds(1.5f);
        textStart.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        yield return new WaitForSeconds(11f);
       // player.CutSceneFinished = true;
        player.IsStationary = false;
        isCutSceneDone = true;
    }

    IEnumerator TextInfoMove()
    {  
       

        StartCoroutine(Fade(1f, 0f, 1f, groupMove));
        yield return new WaitForSeconds(5f);
        StartCoroutine(Fade(1f, 1f, 0f, groupMove));
        yield return new WaitForSeconds(3f);
        StartCoroutine(Fade(1f, 0f, 1f, groupBurst));
        isInfoMoveDone = true;
    }
    IEnumerator TextInfoBurst()
    { 
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(Fade(1f, 1f, 0f, groupBurst));

    }
    IEnumerator FadeText(float duration, float fadeFrom, float fadeTo, TextMeshProUGUI text)
    {
        float timer = 0f;
        Color currentColor = text.color;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeFrom, fadeTo, timer / duration);
          
            text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
    }
    IEnumerator Fade(float duration, float fadeFrom, float fadeTo, CanvasGroup group)
    {
        float timer = 0f;
       
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeFrom, fadeTo, timer / duration);

            group.alpha = alpha;
            yield return null;
        }
    }

    IEnumerator FadeSprite(float duration, float fadeFrom, float fadeTo, SpriteRenderer spr)
    {
        float timer = 0f;
        Color color = spr.color;
       
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeFrom, fadeTo, timer / duration);

            spr.color =new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        
    }
    IEnumerator TransitionBlackScreen(float duration, float fadeFrom, float fadeTo)
    {
        
        float timer = 0f;
        Color currentColor = blackBackground.color;

        foreach (ParticleSystem fly in fireFlies)
        {
            fly.Play();
        }
        
        
         StartCoroutine(FadeSprite(duration, 0f, 1f, foregroundRenderer));
        StartCoroutine(FadeSprite(duration, 0f, 1f, vineRenderer));

        MusicManager.instance.FadeSoundClipMain(4f, 0f, 0.5f);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fadeFrom, fadeTo, timer / duration);
            
            blackBackground.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }
        isFirstCutScene = false;
        blackBackground.gameObject.SetActive(false);
       
    }
   
}
