using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Waiting,
    Chasing,
    Attracting,
    Fleeing
}

//Brain of the enemy
[RequireComponent(typeof(EnemyBehavior))]
public class Enemy : MonoBehaviour
{
    #region Components
    EnemyBehavior enemyBehavior;
    Follower currentFollowerTarget;
    [SerializeField]
    Transform attractAttachPoint;
    #endregion

    [Header("Mouvements")]
    [Range(2f, 20f)]
    [SerializeField]
    float attractSpeed;
    [Range(2f, 20f)]
    [SerializeField]
    float fleeSpeed;

    [SerializeField]
    [Range(3f, 10f)]
    [Tooltip("Durée de fuite lorsque l'enemy se prend une lumière")]
    float inlightFleeDuration;

    Vector3 fleeDirection;

    EnemyState state;


    #region Collisions
    [SerializeField]
    LayerMask blockingChaseMask;

    Collider col;
    float colliderWidth;
    float colliderHeight;

    #endregion

    bool canAttract
    {
        get
        {
            return state != EnemyState.Fleeing && state != EnemyState.Attracting;
        }
    }

    private void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();

        colliderWidth = GetComponent<Collider>().bounds.size.x;
        colliderHeight = GetComponent<Collider>().bounds.size.y;
    }

    private void Update()
    {
        //If he is attracting or fleeing
        if (state == EnemyState.Attracting || state == EnemyState.Fleeing)
        {
            Vector3 moveVelocity = Vector3.zero;

            if (state == EnemyState.Attracting)
                moveVelocity = fleeDirection.normalized * attractSpeed * Time.deltaTime;
            else
                moveVelocity = fleeDirection.normalized * fleeSpeed * Time.deltaTime;

            enemyBehavior.Move(moveVelocity);
            return;
        }

        //If his target is define but becomes inlighted
        if (currentFollowerTarget != null && currentFollowerTarget.IsInlighted)
        {
            currentFollowerTarget = null;
            enemyBehavior.StopDestination(true);
            state = EnemyState.Waiting;
            return;
        }
        //If the enemy doesn't have any target
        if (currentFollowerTarget == null && state == EnemyState.Waiting)
        {
            //Find the closest one
            currentFollowerTarget = FindClosestTargetableFollower();
        }
        //If he has a target, command the behavior TO SEEK FOR THIS FUCKING BITCH
        else if (state == EnemyState.Chasing && currentFollowerTarget != null)
        {
            enemyBehavior.GoToTarget(currentFollowerTarget.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //On collide with a follower, if he can attracts him
        if (other.gameObject.GetComponent<SimpleFollower>() && canAttract)
        {
            //Get the follower to attract
            Follower followerAttracted = other.gameObject.GetComponent<SimpleFollower>();
            currentFollowerTarget = followerAttracted;
            //Remove him from the follower list and change his state
            FindObjectOfType<PlayerLead>().RemoveFollower(followerAttracted);
            followerAttracted.SetState(FollowState.Attracted);
            //The enemy start to attract him in the opposite direction of the leader
            state = EnemyState.Attracting;
            fleeDirection = transform.position - FindObjectOfType<PlayerLead>().transform.position;
            enemyBehavior.StopDestination(true);

            followerAttracted.transform.SetParent(attractAttachPoint);
            followerAttracted.transform.localPosition = Vector3.zero;
        }
        if (other.gameObject.tag == "Light")
        {
            StartCoroutine(FleeAway());
        }
    }

    private Follower FindClosestTargetableFollower()
    {
        //Get the leader followers
        List<Follower> currentFollowers = FindObjectOfType<PlayerLead>().GetFollowers();
        Follower closestTarget = null;
        float closestDist = float.MaxValue;
        //Go through every follower
        foreach (Follower follower in currentFollowers)
        {
            if (!follower.IsInlighted)
            {
                //Find if there is no light between
                RaycastHit hit;
                // Debug.DrawRay(transform.position, follower.transform.position - transform.position, Color.yellow, 1f);
                // Physics.Raycast(transform.position, follower.transform.position - transform.position, out hit, Vector3.Distance(transform.position, follower.transform.position), blockingChaseMask);

                //Cast a capsule from the current collider shape
                Vector3 bottomSphereCenter = new Vector3(transform.position.x, transform.position.y - colliderHeight / 4, transform.position.z);
                Vector3 topSphereCenter = new Vector3(transform.position.x, transform.position.y + colliderHeight / 4, transform.position.z);

                Physics.CapsuleCast(bottomSphereCenter, topSphereCenter, colliderWidth,
                    follower.transform.position - transform.position, out hit,
                    Vector3.Distance(transform.position, follower.transform.position),
                    blockingChaseMask);

                if (hit.collider != null)
                {
                    //Debug.Log("Il y a une lumière !!");
                }
                //Find the closest one
                else if (Vector3.Distance(follower.transform.position, transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(follower.transform.position, transform.position);
                    closestTarget = follower;
                }
            }
        }
        //Target isn't null, enemy start chasing
        if (closestTarget != null)
        {
            state = EnemyState.Chasing;
            if (enemyBehavior.IsNavMeshAgentStopped())
                enemyBehavior.StopDestination(false);
        }

        return closestTarget;
    }

    //Make the ghost flee away, when it is in light for example
    public IEnumerator FleeAway()
    {
        if (state == EnemyState.Attracting)
        {
            //Release current target and set his state at waiting
            currentFollowerTarget.transform.SetParent(null);
            currentFollowerTarget.SetState(FollowState.Waiting);
        }
        if (currentFollowerTarget != null)
            currentFollowerTarget = null;

        state = EnemyState.Fleeing;
        enemyBehavior.StopDestination(true);
        fleeDirection = transform.position - FindObjectOfType<PlayerLead>().transform.position;
        yield return new WaitForSeconds(inlightFleeDuration);
        state = EnemyState.Waiting;
    }

    private void OnBecameInvisible()
    {
        if (state == EnemyState.Attracting)
        {
            Die();
            KillTarget();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void KillTarget()
    {
        Destroy(currentFollowerTarget.gameObject);
    }
}
