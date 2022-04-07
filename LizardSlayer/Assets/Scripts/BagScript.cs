using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagScript : MonoBehaviour , IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] 
    private GameObject slotPreFab;

    private static BagScript instance;
    private CanvasGroup canvasGroup;
    private List<SlotScript> slots = new List<SlotScript>();

    public Vector3 vectorPos;
    public bool IsMove = false;

    public static BagScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BagScript>();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public bool IsOpen
    {
        get { return canvasGroup.alpha > 0; }
    }

    public List<SlotScript> MySlots
    {
        get { return slots; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void AddSlot(int count)
    {
        for(int i = 0;i<count;i++)
        {
            SlotScript slot = Instantiate(slotPreFab, transform).GetComponent<SlotScript>();
            slots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach(SlotScript slot in slots)
        {
            if(slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    public void OnPointerUp(PointerEventData eventData) => InventoryScript.MyInstance.isMove = false;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventoryScript.MyInstance.vectorPos = Input.mousePosition - this.transform.position;
            InventoryScript.MyInstance.isMove = true;
        }
    }
}
