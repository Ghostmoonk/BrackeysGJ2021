using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Component
    [SerializeField]
    InventoryUI inventoryUI;
    #endregion

    Dictionary<ItemType, int> itemsDico;

    public void AddItem(ItemType itemType)
    {
        if (!itemsDico.ContainsKey(itemType))
        {
            itemsDico.Add(itemType, 0);
        }
        itemsDico[itemType] += 1;

        inventoryUI.UpdateItemTextCount(itemType, itemsDico[itemType]);

    }

    public bool RemoveItem(ItemType itemType)
    {
        if (itemsDico.ContainsKey(itemType))
        {
            itemsDico[itemType] -= 1;

            if (itemsDico[itemType] == 0)
            {
                itemsDico.Remove(itemType);
            }
            inventoryUI.UpdateItemTextCount(itemType, itemsDico[itemType]);
            return true;
        }
        else
        {
            return false;
        }
    }


}
