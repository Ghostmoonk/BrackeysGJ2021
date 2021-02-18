using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum EnemyState
{
    Waiting,
    Chasing,
    Attracting,
    Fleeing
}

//Brain of the enemy
[RequireComponent(typeof(EnemyBehavior))]
public class Enemy : MonoBehaviour, IHealth
{
    #region Components
    EnemyBehavior enemyBehavior;
    Follower currentFollowerTarget;
    [SerializeField]
    Transform attractAttachPoint;
    #endregion

    #region Movements
    [Header("Mouvements")]
    [Range(2f, 20f)]
    [SerializeField]
    float attractSpeed;
    [Range(2f, 20f)]
    [SerializeField]
    float fleeSpeed;
    [SerializeField]
    [Range(3f, 10f)]
    [Tooltip("Durée de fuite lorsque l'ennemi se prend une lumière")]
    float inlightFleeDuration;
    #endregion

    [Header("Health")]
    [SerializeField]
    int maxHealth = 1;
    int currentHealth;

    Vector3 fleeDirection;
    EnemyState state;

    #region Collisions
    [Header("Collision")]
    [SerializeField]
    LayerMask blockingChaseMask;
    [SerializeField]
    LayerMask becomeTargetableAreaMask;

    Collider col;
    float colliderWidth;
    float colliderHeight;

    #endregion

    #region Events
    [Header("Events")]
    [SerializeField]
    UnityEvent OnBecomeInvisible;
    #endregion

    bool canAttract
    {
        get
        {
            return state != EnemyState.Fleeing && state != EnemyState.Attracting;
        }
    }

    bool isTargetable = false;

    public bool IsTargetable
    {
        get
        {
            return isTargetable;
        }
    }

    private void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();

        colliderWidth = GetComponent<Collider>().bounds.size.x;
        colliderHeight = GetComponent<Collider>().bounds.size.y;

        currentHealth = maxHealth;
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

        //If his target is define but becomes untargetable
        if (currentFollowerTarget != null && !currentFollowerTarget.IsTargetable)
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
        if (state == EnemyState.Chasing && currentFollowerTarget != null)
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

        //On collide with an "invincible" safe light area
        if (other.gameObject.layer == LayerMask.NameToLayer("InvincibleLight"))
        {
            if (state == EnemyState.Fleeing)
            {
                StopCoroutine(FleeAway());
            }

            StartCoroutine(FleeAway());
        }
        //On collide with an "protected" safe light area
        if (other.gameObject.layer == LayerMask.NameToLayer("ProtectedLight"))
        {
            isTargetable = true;

            //Notify the closest knight that it is in range
            KnightFollower closestKnight = FindClosestKnight();
            if (closestKnight != null)
                closestKnight.OnEnemyInRange(this);

        }
    }

    private KnightFollower FindClosestKnight()
    {
        float minDist = float.MaxValue;
        KnightFollower closestKnight = null;

        foreach (KnightFollower knight in FindObjectsOfType<KnightFollower>())
        {
            float distance = Vector3.Distance(knight.transform.position, transform.position);
            if (distance < minDist)
            {
                closestKnight = knight;
                minDist = distance;
            }
        }
        return closestKnight;
    }

    private bool CheckShouldFlee()
    {
        //Verify if the enemy collider is colliding with an invincible light 
        Vector3 bottomSphereCenter = new Vector3(transform.position.x, transform.position.y - colliderHeight / 4, transform.position.z);
        Vector3 topSphereCenter = new Vector3(transform.position.x, transform.position.y + colliderHeight / 4, transform.position.z);

        Collider[] invincibleLightsCollider = Physics.OverlapCapsule(bottomSphereCenter, topSphereCenter, colliderWidth, blockingChaseMask);

        if (invincibleLightsCollider.Length > 0)
            return true;
        else
            return false;
    }

    private void OnTriggerExit(Collider other)
    {
        //When the enemy get out of the protective light
        if (other.gameObject.tag == "Protected")
        {
            //Check if he is not in another
            //Cast a capsule from the current collider shape
            Vector3 bottomSphereCenter = new Vector3(transform.position.x, transform.position.y - colliderHeight / 4, transform.position.z);
            Vector3 topSphereCenter = new Vector3(transform.position.x, transform.position.y + colliderHeight / 4, transform.position.z);

            RaycastHit hit;
            Physics.CapsuleCast(bottomSphereCenter, topSphereCenter, colliderWidth, Vector3.zero, out hit, 0f,
                    becomeTargetableAreaMask);

            //If it is not in light collider
            if (hit.collider == null)
            {
                isTargetable = false;
                return;
            }
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
            if (follower.IsTargetable)
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
        if (!CheckShouldFlee())
            state = EnemyState.Waiting;
        else
        {
            StartCoroutine(FleeAway());
        }
    }

    private void OnBecameInvisible()
    {
        if (state == EnemyState.Attracting)
        {
            OnBecomeInvisible?.Invoke();
        }
    }

    public void KillTarget()
    {
        Destroy(currentFollowerTarget.gameObject);
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
