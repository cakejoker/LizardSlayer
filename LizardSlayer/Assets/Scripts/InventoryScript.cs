using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;
    public Bag bag;
    private SlotScript fromSlot;

    public Vector3 vectorPos;

    public bool isMove;
    public bool isOnSlot;

    public static InventoryScript MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public SlotScript FromSlot
    {
        get { return fromSlot; }
        set
        {
            fromSlot = value;
            if(value != null)
            {
                fromSlot.MyIcon.color = Color.gray;
            }
        }
    }

    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        bag = (Bag)Instantiate(items[0]);
        bag.Initalize(28);
        bag.Create();
    }

    private void Update()
    {
        if(isMove && !isOnSlot)
        {
            Debug.Log("move");
            this.transform.position = Input.mousePosition - vectorPos;
        }
    }

    public void GetItemData(int itemCode)
    {
        switch(itemCode)
        {
            case 1001:
                Debug.Log("ItemMake");
                LizardTail lizardTail = (LizardTail)Instantiate(items[1]);
                AddItem(lizardTail);
                break;
        }
    }

    public void OpenClose()
    {
        bool closedBag = !bag.MyBagScript.IsOpen;

        if(bag.MyBagScript.IsOpen != closedBag)
        {
            bag.MyBagScript.OpenClose();
        }
    }

    public void AddItem(Item item)
    {
        if(item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
                return;
        }

        PlaceInEmpty(item);
    }

    public int SelectItem(string name)
    {
        int ItemCode = Array.Find(items, item => item.name == name).ItemCode;
        return ItemCode;
    }

    private void PlaceInEmpty(Item item)
    {
        if (bag.MyBagScript.AddItem(item))
            return;
    }

    private bool PlaceInStack(Item item)
    {
        foreach(SlotScript slots in bag.MyBagScript.MySlots)
        {
            if (slots.StackItem(item))
                return true;
        }
        return false;
    }
}
