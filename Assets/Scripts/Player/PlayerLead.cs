using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handle followers
public class PlayerLead : MonoBehaviour
{
    List<Follower> followers;

    Inventory inventory;
    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
    }
    [SerializeField]
    LayerMask followerMask;
    LayerMask enemyMask;

    [SerializeField]
    float appendFollowerRadius = 5f;

    private void Start()
    {
        followers = new List<Follower>();

        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventory.AddItem(ItemType.Sword);
        }
        //If a waiting follower is in range, add him in the follower list
        Collider[] followerColliders = Physics.OverlapSphere(transform.position, appendFollowerRadius, followerMask);
        foreach (Collider followerCollider in followerColliders)
        {
            if (followerCollider.GetComponent<Follower>())
            {
                Follower follower = followerCollider.GetComponent<Follower>();
                if (!followers.Contains(follower) && follower.GetState() == FollowState.Waiting)
                {
                    AppendFollower(follower);
                    follower.SetTarget(transform);
                }
            }
        }

        //Fetch every enemy in the light
        // Collider[] enemiesCollidersInLight = Physics.OverlapSphere(transform.position, lightSphereCol.radius, enemyMask);
        // foreach (Collider enemyCollider in enemiesCollidersInLight)
        // {
        //     if (enemyCollider.GetComponent<Enemy>())
        //     {
        //         Enemy currentEnemyInLight = enemyCollider.GetComponent<Enemy>();
        //         //If an enemy is in the light, make him flee
        //     }
        // }


        // //Get all followers inside the light radius around player
        // Collider[] followersInLight = Physics.OverlapSphere(transform.position, lightSphereCol.radius, followerMask);
        // //Convert this array of colliders to a list of follower
        // List<Follower> followersInLightList = GetFollowersFromTheirColliders(followersInLight);
        // //For every follower, check if it is in the followersInLightList
        // foreach (Follower follower in followers)
        // {
        //     if (followersInLightList.Contains(follower))
        //         follower.SetInlighted(true);
        //     else
        //         follower.SetInlighted(false);
        // }
    }

    private List<Follower> GetFollowersFromTheirColliders(Collider[] colliders)
    {
        List<Follower> followersList = new List<Follower>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Follower>())
            {
                followersList.Add(colliders[i].gameObject.GetComponent<Follower>());
            }
        }

        return followersList;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.8f);
        Gizmos.DrawSphere(transform.position, appendFollowerRadius);

    }
    #region FollowersManagement

    public void AppendFollower(Follower follower)
    {
        followers.Add(follower);
    }

    public void RemoveFollower(Follower follower)
    {
        followers.Remove(follower);
    }

    #endregion

    #region Light

    public List<Follower> GetFollowers()
    {
        return followers;
    }

    #endregion
}