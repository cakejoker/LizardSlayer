using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag",order = 1)]
public class Bag : Item, IUseable
{
    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }
    public Sprite MyIcon { get; }

    public int Slots
    {
        get { return slots; }
    }
    
    public void Initalize(int slots)
    {
        this.slots = slots;
    }

    public void Create()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlot(slots);
    }
}
