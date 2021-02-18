using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragItemUI : MonoBehaviour
{
    [SerializeField]
    LayerMask dragItemMask;

    ItemType itemType;
    [SerializeField]
    Image image;

    public void Setup(ItemType _itemType, Sprite _sprite)
    {
        itemType = _itemType;
        image.sprite = _sprite;
    }

    public void Release()
    {
        //When release, cast a ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, float.MaxValue, dragItemMask);
        if (hit.collider != null)
            //If it's on a simple follower
            if (hit.collider.GetComponent<SimpleFollower>())
            {
                SimpleFollower followerToUpgrade = hit.collider.GetComponent<SimpleFollower>();
                if (followerToUpgrade.CanBeUpgraded())
                {
                    FollowerUpgrader.Instance.UpgradeFollower(followerToUpgrade, itemType);
                    Debug.Log(hit.collider.name);
                }
            }
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
