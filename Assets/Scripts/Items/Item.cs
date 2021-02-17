using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemType itemType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }


}

public enum ItemType
{
    Sword,
    Torch
}