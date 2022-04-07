using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            string itemName = gameObject.name.Replace("(Clone)","");
            int itemCode = InventoryScript.MyInstance.SelectItem(itemName);
            InventoryScript.MyInstance.GetItemData(itemCode);
            Destroy(this.gameObject);
        }
    }
}
