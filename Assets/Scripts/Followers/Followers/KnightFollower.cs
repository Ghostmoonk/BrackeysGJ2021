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
public class KnightFollower : Follower
{
    FighterBehavior fighterBehavior;
    KnightAttackTrigger knightAttackTrigger;

    [Header("Health")]
    [Tooltip("Nombre de fois que le chevalier va pouvoir tanker")]
    [SerializeField]
    UnityEvent OnLastHeadRemoved;

    [Header("Attack")]
    [SerializeField]
    float maxSpotingRangeRadius = 20f;
    public float MaxSpotingRangeRadius
    {
        get
        {
            return maxSpotingRangeRadius;
        }
    }

    [SerializeField]
    float stoppingDistToEnemy = 1f;
    [SerializeField]
    int attackDamages;
    [SerializeField]
    LayerMask enemyMask;

    [SerializeField]
    GameObject SimpleFollowerPrefab;

    public delegate void EnemyInRangeDelegate(Enemy enemy);
    public EnemyInRangeDelegate OnEnemyInRange;
    public EnemyInRangeDelegate OnEnemyOutOfRange;

    protected override void Start()
    {
        base.Start();
        fighterBehavior = (FighterBehavior)followBehavior;
        knightAttackTrigger = GetComponentInChildren<KnightAttackTrigger>();

        //Delegates
        OnEnemyInRange += SetTarget;
        OnEnemyOutOfRange += UnsetEnemyTarget;
        knightAttackTrigger.EnemyAttackable += PerformAttack;
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
            //He is not at destination, if it is stopped, activate it and go to target
            if (!followBehavior.IsNavMeshEnabled())
                followBehavior.ToggleNavMesh(true);

            followBehavior.GoToTarget(currentTarget);
        }
    }

    public void PerformAttack(Enemy enemy)
    {
        fighterBehavior.Attack(enemy, attackDamages);
        UpdateHealth(-1);
        SetTarget(FindObjectOfType<PlayerLead>().transform);
    }

    public void SetTarget(Enemy enemy)
    {
        currentTarget = enemy.transform;
        fighterBehavior.SetNewStoppingDist(stoppingDistToEnemy);
    }

    public override void SetTarget(Transform _target)
    {
        followState = FollowState.Following;
        base.SetTarget(_target);
        if (fighterBehavior != null)
            fighterBehavior.ResetStoppingDistance();
    }

    public void UnsetEnemyTarget(Enemy enemy)
    {
        if (currentTarget == enemy.transform)
        {
            SetTarget(FindObjectOfType<PlayerLead>().transform);
        }
    }

    public void SpawnFollower()
    {
        Follower followerToReplace = Instantiate(SimpleFollowerPrefab, transform.position, transform.rotation, GameObject.FindGameObjectWithTag("Followers").transform).GetComponent<Follower>();
        followerToReplace.SetTarget(FindObjectOfType<PlayerLead>().transform);
        followerToReplace.SetState(FollowState.Following);

        FindObjectOfType<PlayerLead>().RemoveFollower(this);
        Destroy(gameObject);
    }

    public new void Die()
    {
        SpawnFollower();
        Destroy(gameObject);
    }

    public new void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            OnLastHeadRemoved?.Invoke();
        }
    }
}

public interface IHealth
{
    void UpdateHealth(int amount);
    void Die();
    int GetCurrentHealth();
}