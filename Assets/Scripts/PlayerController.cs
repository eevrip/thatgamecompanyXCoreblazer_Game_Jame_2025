using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    private static int lengthOfSequence;
    public static int LengthOfSequence {  get { return lengthOfSequence; } set { lengthOfSequence = value; } }
    private static bool isRecordingSequence;
    public static bool IsRecordingSequence { get { return isRecordingSequence; } set { isRecordingSequence = value; } }


    [SerializeField] private List<Color> color;
    [SerializeField] private List<Gradient> gradient;
    [SerializeField] private List<AudioClip> soundClip;
    [SerializeField] private AudioClip [] gettingNewColorClip;
    [SerializeField] private ParticleSystem trailParticle;
    [SerializeField] private ParticleSystem burstingParticle;
    [SerializeField] private ParticleSystem newColorParticle;
    [SerializeField] private Light2D spotLight;
    [SerializeField] private SpriteRenderer lightSprite;
    [SerializeField] private SpriteRenderer burstSprite;
    [SerializeField] private Animator animator;
    public Animator PlayerAnimator { get { return animator; } }
    [SerializeField] private List<int> availableLights = new List<int>();
    private List<int> colorSequence = new List<int>();


    private int currentLight = 0;
    private int lastIdxColor = 0;
    private float alphaBurstingColor;
    private Item currentItem;
    public Item CurrentItem { get { return currentItem; } set { currentItem = value; } }
    public List<Color> CurrentColor { get { return color; } set { color = value; } }
    public List<Gradient> CurrentGradient { get { return gradient; } set { gradient = value; } }

    private bool isBursting;
    public bool IsBursting { get { return isBursting; } }


    public delegate void OnInventoryUpdate();
    public OnInventoryUpdate onInventoryCallback;

    public delegate void OnSequenceUpdate();
    public OnSequenceUpdate onSequenceCallback;


    public delegate void OnBurstingUpdate();
    public OnBurstingUpdate onBurstingCallback;
    private bool isStatic;
    public bool IsStatic { get { return isStatic; } set { isStatic = value; } }

    private bool isStationary = true;
    public bool IsStationary { get { return isStationary; } set { isStationary = value; } }

    private bool isColorRegistering = true;
    public bool IsColorRegistering { get { return isColorRegistering;  } set { isColorRegistering = value; } }

    private bool cutSceneFinished;

    public bool CutSceneFinished { get { return cutSceneFinished; } set { cutSceneFinished = value; } }
    [SerializeField] private bool isMovingMouse;
    public bool IsMovingMouse { get { return isMovingMouse; } set { isMovingMouse = value; } }
    void Start()
    {
      
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
       ClearColorSequence();
        

    }
   



    public void ActivateTrail()
    {
        trailParticle.gameObject.SetActive(true);
    }
    public void DeactivateTrail()
    {
        trailParticle.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!isMovingMouse && !MenuManager.isGamePaused)
        {

            if (!isStationary && !cutSceneFinished)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                cutSceneFinished = true;


            }


            if (!isStationary && cutSceneFinished)
            {

                Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

                transform.position = new Vector3(mousePos.x, mousePos.y, 0f);

            }







            if (burstSprite.gameObject.activeSelf)
            {
                alphaBurstingColor = animator.GetFloat("BurstingAlpha");
                burstSprite.color = new Color(color[currentLight].r, color[currentLight].g, color[currentLight].b, alphaBurstingColor);


            }
            //Changing mode and bursting
            if (availableLights.Count > 1)
            {
                //Change Color

                if (Input.GetKeyDown(KeyCode.Space) && !isStatic)
                {
                    ToolTipManager.instance.DeactivateInformation(2);
                    if (lastIdxColor < availableLights.Count - 1)
                        lastIdxColor++;
                    else
                        lastIdxColor = 0;

                    alphaBurstingColor = animator.GetFloat("BurstingAlpha");

                    currentLight = availableLights[lastIdxColor];
                    var colorModuleTrail = trailParticle.colorOverLifetime;
                    colorModuleTrail.enabled = true;
                    var colorModuleBurst = burstingParticle.colorOverLifetime;

                    // Set a new gradient
                    colorModuleTrail.color = new ParticleSystem.MinMaxGradient(gradient[currentLight]);
                    colorModuleBurst.color = new ParticleSystem.MinMaxGradient(gradient[currentLight]);
                    spotLight.color = color[currentLight];
                    lightSprite.color = color[currentLight];
                    burstSprite.color = new Color(color[currentLight].r, color[currentLight].g, color[currentLight].b, alphaBurstingColor);


                    //Animation of alpha and intensity

                }
            }

            if (availableLights.Count >= 1)
            {
                if (CutsceneManager.instance.CanBurst && Input.GetMouseButtonDown(1) && !isStatic)//Right click
                {
                    ChangeSequence();


                    SFXManager.instance.PlayClip(0, soundClip[currentLight], 0.8f);
                    if (onBurstingCallback != null)
                        onBurstingCallback.Invoke();
                    //StartCoroutine(Bursting());
                    animator.SetTrigger("burst");


                }

            }
        }
    }

    public void Burst()
    {
        SFXManager.instance.PlayClip(0, soundClip[currentLight], 0.8f);
        if (onBurstingCallback != null)
            onBurstingCallback.Invoke();
        //StartCoroutine(Bursting());
        animator.SetTrigger("burst");
    }

    public void AddToAvailableLights(int cat)
    {
        Debug.Log("add color");
        availableLights.Add(cat);
        var colorEffect = newColorParticle.colorOverLifetime;
        colorEffect.color = new ParticleSystem.MinMaxGradient(gradient[cat]);
       
        int randInt = Random.Range(0, gettingNewColorClip.Length);
        SFXManager.instance.PlayClip(0, gettingNewColorClip[randInt], 0.5f);
        newColorParticle.Play();
        //trailParticle.Stop();
        //PlaySound
        StartCoroutine(ChangeColor(color[currentLight], color[cat], 8f));
        lastIdxColor = cat;
        currentLight = cat;
       
    }

    public void GetItem(Item item)
    {
        StartCoroutine(DelayToGetItem(2f, item));
     /*   currentItem = item;
        Debug.Log(item);
        if (onInventoryCallback != null)
            onInventoryCallback.Invoke();*/
    }
    public void GiveItem()
    {
        currentItem = null;
        if (onInventoryCallback != null)
            onInventoryCallback.Invoke();
    }

   
    public void ChangeSequence()
    {
        if (isRecordingSequence && isColorRegistering)
        {
            
            if (colorSequence.Count < lengthOfSequence)
            {
               
                colorSequence.Add(currentLight);
                Debug.Log("register " + colorSequence.Count );
                if(colorSequence.Count == lengthOfSequence) { 
                   
                    if (onSequenceCallback != null)
                        onSequenceCallback.Invoke();
                }
               
            }
            else
            {
               
                ClearColorSequence();
            }


        }
        else
            {
              /*  colorSequence.RemoveAt(0);
                colorSequence.Add(currentLight);
                if(onSequenceCallback!=null)
                    onSequenceCallback.Invoke();*/
             ClearColorSequence();
            }
        
    }
    public List<int> GetColorSequence()
    {
        return colorSequence;
    }
    public void ClearColorSequence()
    {
        colorSequence.Clear();
    }
    IEnumerator DelayToGetItem(float duration, Item item)
    {
        yield return new WaitForSeconds(duration);
        currentItem = item;
        Debug.Log(item);
        if (onInventoryCallback != null)
            onInventoryCallback.Invoke();
    }
    IEnumerator ChangeColor(Color from, Color to, float duration)
    {
      
        //  yield return new WaitForSeconds(0.2f);
        //isStatic = true;


        float timer = 0;

       


        while (timer < duration)
        {
            timer += Time.deltaTime;
            Color colorCur = Color.Lerp(from, to, timer / duration);
            spotLight.color = colorCur;
            lightSprite.color = colorCur;
            
            yield return null;
        }
       
        var colorModuleTrail = trailParticle.colorOverLifetime;
        colorModuleTrail.enabled = true;
        var colorModuleBurst = burstingParticle.colorOverLifetime;

        // Set a new gradient
        colorModuleTrail.color = new ParticleSystem.MinMaxGradient(gradient[currentLight]);
        colorModuleBurst.color = new ParticleSystem.MinMaxGradient(gradient[currentLight]);

        burstSprite.color = new Color(color[currentLight].r, color[currentLight].g, color[currentLight].b, alphaBurstingColor);
        //trailParticle.Play();
       // isStatic = false;
        
    }
    IEnumerator Bursting()
    {
        isBursting = true;
        yield return new WaitForSeconds(1f);
        isBursting = false;
    }
    public enum CatsEyes
    {
        Yellow,
        Pink, Blue,
        Green,
        Orange,
        Purple,
        White
    }
}



