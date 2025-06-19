using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    [SerializeField] private GameObject kitchen;
    [SerializeField] private GameObject fishCooked;
    [SerializeField] private GameObject fishEaten;
    [SerializeField] private Color dimColor;
    [SerializeField] private Clock clock;
    [SerializeField] private AudioClip clipWashing;

    private void Start()
    {
        clock.onClockcallback += EnableFishEaten;
    }

    public void EnableFishEaten()
    {
        kitchen.GetComponent<SpriteRenderer>().color = dimColor;
        fishCooked.GetComponent<SpriteRenderer>().color = dimColor;
        fishEaten.GetComponent<SpriteRenderer>().color= dimColor;
        fishCooked.SetActive(false);
        fishEaten.SetActive(true);
        EnvironmentManager.instance.clipKitchen = clipWashing;
    }
}
