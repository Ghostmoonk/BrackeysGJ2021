using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Knight is a follower that can
// - Seek for targetable enemies
// - Move to them
// - Attack them and kill them when at range
//Knight has
// - Health, determine the amount of attack he cans deal. At 0, it becomes a simple follower
// - An attack range, if an enemy is inside, fire attack

[RequireComponent(typeof(FighterBehavior))]
public class KnightFollower : Follower, IHealth
{
    FighterBehavior fighterBehavior;

    [Header("Health")]
    [Tooltip("Nombre de fois que le chevalier va pouvoir tanker")]
    [SerializeField]
    int maxHealth;
    int currentHealth;
    [SerializeField]
    UnityEvent OnLastHeadRemoved;

    [Header("Attack")]
    [SerializeField]
    float attackRange;
    [SerializeField]
    LayerMask enemyMask;

    [SerializeField]
    GameObject SimpleFollowerPrefab;

    public delegate void EnemyInRangeDelegate(Enemy enemy);
    public EnemyInRangeDelegate OnEnemyInRange;

    protected override void Start()
    {
        base.Start();
        fighterBehavior = (FighterBehavior)followBehavior;

        OnEnemyInRange += SetTarget;
    }

    protected override void Update()
    {

        //If the follower waits and his navmesh is disable, reactivate it
        if (followState == FollowState.Waiting && !followBehavior.IsNavMeshEnabled())
        {
            fighterBehavior.ToggleNavMesh(true);
        }

        //If the follower has a target and is following
        if (currentTarget != null && followState == FollowState.Following)
        {
            //If he is already at destination, return
            if (followBehavior.CheckDestinationReached(currentTarget))
                return;

            //He is not at destination, if it is stopped, activate it and go to target
            if (!followBehavior.IsNavMeshEnabled())
                followBehavior.ToggleNavMesh(true);

            followBehavior.GoToTarget(currentTarget);
        }
    }

    private void FixedUpdate()
    {
        //Every frame, raycast in front of him to check if there is an enemy to attack
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, attackRange, enemyMask);

        //Knight has an enemy in range
        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (enemy != null)
                fighterBehavior.Attack(enemy);
        }
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
        {
            OnLastHeadRemoved?.Invoke();
        }
    }

    public void SetTarget(Enemy enemy)
    {
        currentTarget = enemy.transform;
    }

    public void SpawnFollower()
    {
        Follower followerToReplace = Instantiate(SimpleFollowerPrefab, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Followers").transform).GetComponent<Follower>();
        followerToReplace.SetTarget(FindObjectOfType<PlayerLead>().transform);
        followerToReplace.SetState(FollowState.Following);

        FindObjectOfType<PlayerLead>().RemoveFollower(this);
        Destroy(gameObject);
    }

    public void Die()
    {
        SpawnFollower();
        Destroy(gameObject);
    }
}

public interface IHealth
{
    void UpdateHealth(int amount);
    void Die();
}