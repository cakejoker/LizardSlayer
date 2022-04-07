using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerClickHandler,IClickable, IPointerEnterHandler, IPointerExitHandler 
    ,IPointerDownHandler,IPointerUpHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;
    public Text MyStackText
    {
        get { return stackSize; }
    }

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public Image MyIcon
    {
        get { return icon; }
        set { icon = value; }
    }

    public int MyCount
    {
        get { return items.Count; }
    }

    public Item MyItem
    {
        get
        {
            if(!IsEmpty)
            {
                return items.Peek();
            }
            return null;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
                return false;
            else
                return true;
        }
    }

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot);
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)
    {
        items.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        return true;
    }

    public bool StackItem(Item item)
    {
        if(!IsEmpty && item.name == MyItem.name)
        {
            if(items.Count < MyItem.MyStackSize)
            {
                items.Push(item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(Item item)
    {
        if(!IsEmpty)
        {
            items.Pop();
        }
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    private bool PutItemBack()
    {
        if(InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
            return false;

        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tempFrom = new ObservableStack<Item>(from.items);

            from.items.Clear();
            from.AddItems(items);

            items.Clear();

            AddItems(tempFrom);

            return true;
        }
        return false;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if(IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for(int i = 0;i<count;i++)
            {
                if (IsFull)
                    return false;

                AddItem(newItems.Pop());
            }
            return true;
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click1");
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Click2");
            if(InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                InventoryScript.MyInstance.FromSlot = this;
            }
            else if(InventoryScript.MyInstance.FromSlot != null)
            {
                if(PutItemBack() || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.items))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                if (items.Count >= 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        items.Pop();
                    }
                    GameManager.MyInstance.score += 20;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData) => InventoryScript.MyInstance.isOnSlot = false;

    public void OnPointerDown(PointerEventData eventData) => InventoryScript.MyInstance.isOnSlot = true;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(transform.position,MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
