using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerUpgrader : MonoBehaviour
{
    private static FollowerUpgrader instance;
    public static FollowerUpgrader Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [SerializeField]
    FollowerItem[] followersItem;
    Dictionary<ItemType, GameObject> followersItemDico;

    PlayerLead playerLead;

    private void Start()
    {
        playerLead = FindObjectOfType<PlayerLead>();
        followersItemDico = new Dictionary<ItemType, GameObject>();
        foreach (FollowerItem followerItem in followersItem)
        {
            followersItemDico.Add(followerItem.itemType, followerItem.followerPrefab);
        }
    }

    //Upgrade follower = remove the follower, add a new one depending on item type
    public void UpgradeFollower(Follower follower, ItemType itemType)
    {
        Debug.Log("Upgrade le follower " + follower.name + " avec une : " + itemType.ToString());
        Follower newFollowerToInstantiate = Instantiate(followersItemDico[itemType],
            follower.transform.position,
            follower.transform.rotation,
            GameObject.FindGameObjectWithTag("Followers").transform).GetComponent<Follower>();

        newFollowerToInstantiate.SetTarget(follower.CurrentTarget);
        playerLead.AppendFollower(newFollowerToInstantiate);

        playerLead.RemoveFollower(follower);
        Destroy(follower.gameObject);
    }
}

[System.Serializable]
public struct FollowerItem
{
    public GameObject followerPrefab;
    public ItemType itemType;
}
