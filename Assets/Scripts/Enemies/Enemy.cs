using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;


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
    public EnemyState state;

    #region Collisions
    [Header("Collision")]
    [SerializeField]
    LayerMask blockingChaseMask;
    [SerializeField]
    LayerMask becomeTargetableAreaMask;

    float colliderWidth;
    float colliderHeight;

    #endregion

    #region Events
    //[Header("Events")]
    //[SerializeField]
    //UnityEvent OnBecomeInvisible;

    public delegate void OnDie(Enemy enemy);
    public static event OnDie OnDieEvent;

    #endregion

    [SerializeField]
    float attractTimeToKill = 5f;

    KnightFollower knightFollowing;

    bool canAttract
    {
        get
        {
            return state != EnemyState.Fleeing && state != EnemyState.Attracting;
        }
    }

    bool canChase
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

    #region Sound
    [Header("Sound")]
    [SerializeField]
    AudioSource idleSound;
    [SerializeField]
    AudioSource spawnSound;
    [SerializeField]
    string idleSoundName;
    #endregion

    private void Start()
    {
        enemyBehavior = GetComponent<EnemyBehavior>();
        colliderWidth = GetComponent<Collider>().bounds.size.x;
        colliderHeight = GetComponent<Collider>().bounds.size.y;

        currentHealth = maxHealth;

        PoofManager.Instance.InstantiatePoof(transform);
        SoundManager.Instance.PlaySound(spawnSound, "enemy-spawn");
        InvokeRepeating("PlayEnemyIdleSound", 0.001f, Random.Range(2f, 10f));
    }

    public void PlayEnemyIdleSound()
    {
        SoundManager.Instance.PlaySound(idleSound, idleSoundName);
    }

    public void ClearTarget()
    {
        currentFollowerTarget = null;

        if (state == EnemyState.Chasing)
        {
            state = EnemyState.Waiting;
        }
    }

    float attrackingTimer = 0f;

    private void Update()
    {
        //If he is attracting or fleeing
        if (state == EnemyState.Attracting || state == EnemyState.Fleeing)
        {
            Vector3 moveVelocity = Vector3.zero;

            if (state == EnemyState.Attracting)
            {
                moveVelocity = fleeDirection.normalized * attractSpeed * Time.deltaTime;
                attrackingTimer += Time.deltaTime;

                if (attrackingTimer >= attractTimeToKill)
                {
                    KillTarget();
                    Die();
                    return;
                }
            }
            else
                moveVelocity = fleeDirection.normalized * fleeSpeed * Time.deltaTime;
            enemyBehavior.StopDestination(true);
            //enemyBehavior.GoToTarget(fleeDirection.normalized * 2f);
            //enemyBehavior.Move(moveVelocity);
            enemyBehavior.MoveVelocity(moveVelocity * 100f);
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
        //If he has a target, command the behavior search for
        if (canChase && currentFollowerTarget != null)
        {
            enemyBehavior.GoToTarget(currentFollowerTarget.transform);
        }

        if (state == EnemyState.Chasing && currentFollowerTarget == null)
        {
            state = EnemyState.Waiting;
            enemyBehavior.StopDestination(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //On collide with a follower, if he can attracts him
        if (other.gameObject.GetComponent<SimpleFollower>() && canAttract)
        {
            if (other.gameObject.GetComponent<SimpleFollower>().CanBeAttracted())
            {
                //Get the follower to attract
                Follower followerAttracted = other.gameObject.GetComponent<SimpleFollower>();
                currentFollowerTarget = followerAttracted;

                //Remove him from the follower list and change his state
                FindObjectOfType<PlayerLead>().RemoveFollower(followerAttracted);
                followerAttracted.SetState(FollowState.Attracted);

                //The enemy start to attract him in the opposite direction of the leader
                state = EnemyState.Attracting;
                fleeDirection = new Vector3(
                    transform.position.x - FindObjectOfType<PlayerLead>().transform.position.x,
                    0f,
                    transform.position.z - FindObjectOfType<PlayerLead>().transform.position.z);

                //enemyBehavior.GoToTarget(fleeDirection.normalized * Mathf.Infinity);
                enemyBehavior.SetNewSpeed(attractSpeed);
                enemyBehavior.StopDestination(true);

                followerAttracted.transform.SetParent(attractAttachPoint);
                followerAttracted.transform.localPosition = Vector3.zero;

                //Notify all enemy to stop target this follower
                foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    if (enemy.GetCurrentFollowerTarget() && enemy != this)
                    {
                        enemy.ClearTarget();
                    }
                }

                return;
            }
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
        //On collide with a "protected" safe light area
        if (becomeTargetableAreaMask == (becomeTargetableAreaMask | (1 << other.gameObject.layer)))
        {
            isTargetable = true;

            //Notify the closest knight that it is in range
            KnightFollower closestKnight = FindClosestKnight();
            if (closestKnight != null)
            {
                closestKnight.OnEnemyInRange(this);
                knightFollowing = closestKnight;
            }
        }
    }

    public Follower GetCurrentFollowerTarget() => currentFollowerTarget;

    private KnightFollower FindClosestKnight()
    {
        float minDist = float.MaxValue;
        KnightFollower closestKnight = null;

        foreach (KnightFollower knight in FindObjectsOfType<KnightFollower>())
        {
            float distance = Vector3.Distance(knight.transform.position, transform.position);
            if (distance < minDist && distance < knight.MaxSpotingRangeRadius)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("ProtectedLight"))
        {
            isTargetable = false;
            if (knightFollowing != null)
            {
                knightFollowing.OnEnemyOutOfRange(this);
                knightFollowing = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //When he goes out of protective light and stay on a protective light
        if (becomeTargetableAreaMask == (becomeTargetableAreaMask | (1 << other.gameObject.layer)) && !isTargetable)
        {
            if (!isTargetable)
                isTargetable = true;
            //Notify the closest knight that it is in range
            if (knightFollowing == null)
            {
                KnightFollower closestKnight = FindClosestKnight();

                if (closestKnight != null)
                    closestKnight.OnEnemyInRange(this);
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
            if (follower.IsTargetable && follower.GetType() == typeof(SimpleFollower))
            {
                RaycastHit hit;
                //Find if there is no light between
                //Cast a capsule from the current collider shape
                Vector3 bottomSphereCenter = new Vector3(transform.position.x, transform.position.y - colliderHeight / 4, transform.position.z);
                Vector3 topSphereCenter = new Vector3(transform.position.x, transform.position.y + colliderHeight / 4, transform.position.z);

                Physics.CapsuleCast(bottomSphereCenter, topSphereCenter, colliderWidth,
                    follower.transform.position - transform.position, out hit,
                    Vector3.Distance(transform.position, follower.transform.position),
                    blockingChaseMask);

                if (hit.collider != null)
                {
                    //Debug.Log("Il y a une lumière !! Je peux pas target");
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
        if (state == EnemyState.Attracting && currentFollowerTarget != null)
        {
            ReleaseCurrentAttractingTarget();
        }
        if (currentFollowerTarget != null)
            currentFollowerTarget = null;

        state = EnemyState.Fleeing;
        //enemyBehavior.StopDestination(true);
        fleeDirection = fleeDirection = new Vector3(
                    transform.position.x - FindObjectOfType<PlayerLead>().transform.position.x,
                    0f,
                    transform.position.z - FindObjectOfType<PlayerLead>().transform.position.z);

        //enemyBehavior.GoToTarget(fleeDirection.normalized * Mathf.Infinity);
        enemyBehavior.SetNewSpeed(fleeSpeed);

        yield return new WaitForSeconds(inlightFleeDuration);
        if (!CheckShouldFlee())
        {
            state = EnemyState.Waiting;
            enemyBehavior.ResetSpeed();
        }
        else
        {
            StartCoroutine(FleeAway());
        }
    }

    public void ReleaseCurrentAttractingTarget()
    {
        //Release current target and set his state at waiting
        currentFollowerTarget.transform.SetParent(GameObject.FindGameObjectWithTag("Followers").transform);
        currentFollowerTarget.SetState(FollowState.Waiting);
        currentFollowerTarget.transform.position = attractAttachPoint.position;

        //currentFollowerTarget.transform.position = new Vector3(attractAttachPoint.position.x, 3f, attractAttachPoint.position.z);

        attrackingTimer = 0f;
    }

    public void KillTarget()
    {
        if (currentFollowerTarget != null)
        {
            currentFollowerTarget.Die();
            currentFollowerTarget = null;
        }

    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDieEvent?.Invoke(this);
        PoofManager.Instance.InstantiatePoof(transform);
        if (currentFollowerTarget != null)
        {
            ReleaseCurrentAttractingTarget();
        }
        SoundManager.Instance.PlaySound(spawnSound, "enemy-die");
        Destroy(gameObject);
    }

    public int GetCurrentHealth() => currentHealth;
}
