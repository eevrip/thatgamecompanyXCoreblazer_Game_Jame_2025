using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemObject")]
public class Item : ScriptableObject
{
   
    public enum Type
    {
        None,
        Bedsheet,
        Box,
        Key,
        Ladder,
        ClockHands,
        Fish,
        Toy
        
    }
    [SerializeField] private Sprite uiSprite;
    public Sprite UiSprite { get { return uiSprite; } }
    public Type type;
    public string itemName;
    // Start is called before the first frame update


    
}
