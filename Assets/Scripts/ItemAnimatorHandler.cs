using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimatorHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Animator animator;
    [SerializeField] private Cat cat;
    public void OnEnable()
    {
        if (cat.IsHappy)
        {
            ItemWasGiven();
        }
    }
    public void ToGiveItem()
    {
        animator.SetTrigger("giveItem");
    }
    public void ItemWasGiven()
    {
        animator.SetBool("newPos", true);
    }
}
