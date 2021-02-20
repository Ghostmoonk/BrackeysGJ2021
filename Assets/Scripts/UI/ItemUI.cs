using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemUI : MonoBehaviour
{
    [SerializeField]
    GameObject dragItemPrefab;
    [SerializeField]
    ItemType itemType;

    public void SpawnDragguedItem()
    {
        //If we have the item, allow to drag it
        if (FindObjectOfType<PlayerLead>().Inventory.GetItemAmount(itemType) > 0)
        {
            DragItemUI dragItemUIToSpawn = Instantiate(dragItemPrefab, GetComponentInParent<Canvas>().transform).GetComponent<DragItemUI>();
            dragItemUIToSpawn.Setup(itemType, GetComponent<Image>().sprite);
        }
    }
}
