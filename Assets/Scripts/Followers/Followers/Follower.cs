using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum FollowState
{
    Waiting,
    Following,
    Attracted
}

[RequireComponent(typeof(FollowerBehavior))]
public abstract class Follower : MonoBehaviour, IHealth
{
    #region Components
    protected FollowerBehavior followBehavior;
    #endregion

    [SerializeField]
    protected int maxHealth;
    protected int currentHealth;

    protected FollowState followState = FollowState.Waiting;
    protected Transform currentTarget;
    public Transform CurrentTarget
    {
        get
        {
            return currentTarget;
        }
    }

    protected bool isTargetable;
    public bool IsTargetable
    {
        get
        {
            return isTargetable;
        }
    }

    protected virtual void Start()
    {
        followBehavior = GetComponent<FollowerBehavior>();

        isTargetable = true;

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        //If the navmesh isn't stop and he is getting attracted
        if (followBehavior.IsNavMeshEnabled() && followState == FollowState.Attracted)
        {
            //Disable navMesh
            followBehavior.ToggleNavMesh(false);
            return;
        }

        if (followState == FollowState.Attracted && !GetComponent<Rigidbody>().IsSleeping())
        {
            GetComponent<Rigidbody>().Sleep();
        }
        else if (followState != FollowState.Attracted && GetComponent<Rigidbody>().IsSleeping())
        {
            GetComponent<Rigidbody>().WakeUp();
        }

        //If the follower waits and his navmesh is disable, reactivate it
        if (followState == FollowState.Waiting && !followBehavior.IsNavMeshEnabled())
        {
            followBehavior.ToggleNavMesh(true);
        }

        //If the follower has a target and is following
        if (currentTarget != null && followState == FollowState.Following)
        {
            //If he is already at destination, return
            //if (followBehavior.CheckDestinationReached(currentTarget))
            //    return;

            //He is not at destination, if it is stopped, activate it and go to target
            if (!followBehavior.IsNavMeshEnabled())
                followBehavior.ToggleNavMesh(true);

            followBehavior.GoToTarget(currentTarget);
        }
    }

    //Set the target to the current target. Can be null
    public virtual void SetTarget(Transform _target)
    {
        currentTarget = _target;
        SetState(FollowState.Following);
    }

    public void SetTargetable(bool _targetable)
    {
        isTargetable = _targetable;
    }

    public void SetState(FollowState _newState) => followState = _newState;

    public FollowState GetState() => followState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InvincibleLight"))
        {
            isTargetable = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("InvincibleLight"))
        {
            isTargetable = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Invincible")
        {
            if (isTargetable)
                isTargetable = false;
        }
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public int GetCurrentHealth() => currentHealth;

    public void Die()
    {
        PoofManager.Instance.InstantiatePoof(transform);
        Destroy(gameObject);
    }
}