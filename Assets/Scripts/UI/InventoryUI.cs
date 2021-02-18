using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI swordText;
    [SerializeField]
    TextMeshProUGUI torchText;

    private void Start()
    {
        swordText.text = "0";
        torchText.text = "0";
    }

    public void UpdateItemTextCount(ItemType itemType, int newCount)
    {
        switch (itemType)
        {
            case ItemType.Sword:
                swordText.text = newCount.ToString();
                break;
            case ItemType.Torch:
                torchText.text = newCount.ToString();
                break;
            default:
                break;
        }
    }

}
