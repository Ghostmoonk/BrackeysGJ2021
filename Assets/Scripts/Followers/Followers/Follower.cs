using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FollowState
{
    Waiting,
    Following,
    Attracted
}

[RequireComponent(typeof(FollowerBehavior))]
public abstract class Follower : MonoBehaviour
{
    #region Components

    protected FollowerBehavior followBehavior;

    #endregion

    protected FollowState followState;
    protected Transform currentTarget;

    protected bool isInlighted;
    public bool IsInlighted
    {
        get
        {
            return isInlighted;
        }
    }

    protected virtual void Start()
    {
        followBehavior = GetComponent<FollowerBehavior>();

        isInlighted = false;
        followState = FollowState.Waiting;
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

        //If the follower waits and his navmesh is disable, reactivate it
        if (followState == FollowState.Waiting && !followBehavior.IsNavMeshEnabled())
        {
            followBehavior.ToggleNavMesh(true);
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

    //Set the target to the current target. Can be null
    public void SetTarget(Transform _target)
    {
        currentTarget = _target;
        SetState(FollowState.Following);
    }

    public void SetInlighted(bool _inLight)
    {
        isInlighted = _inLight;
    }

    public void SetState(FollowState _newState) => followState = _newState;

    public FollowState GetState() => followState;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            isInlighted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            isInlighted = false;
        }
    }
}