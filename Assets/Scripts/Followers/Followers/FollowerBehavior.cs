using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Gere le comportement des followers
[RequireComponent(typeof(NavMeshAgent))]
public class FollowerBehavior : MonoBehaviour
{
    #region Components

    protected NavMeshAgent agent;
    #endregion

    protected Follower followerScript;
    float destinationReachedThreshold = 3f;
    [HideInInspector]
    public Animator animator;
    //[SerializeField]
    //float destinationReachedThreshold = 3f;
    protected virtual void Start()
    {
        followerScript = GetComponent<Follower>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetFloat("Move", agent.velocity.magnitude);

            if (followerScript.GetState() == FollowState.Attracted)
            {
                transform.rotation = Quaternion.identity;
                Debug.Log(animator.GetBool("isDragged"));
                animator.SetBool("isDragged", true);
            }
            else
            {
                animator.SetBool("isDragged", false);
            }

            if (followerScript.GetState() == FollowState.Following)
            {
                animator.SetBool("isFollower", true);

            }
            else
            {
                animator.SetBool("isFollower", false);
            }
        }
    }

    public void GoToTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }

    public void StopDestination(bool _switch)
    {
        if (_switch)
        {
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
        //agent.isStopped = _switch;
    }

    public void ToggleNavMesh(bool toggle)
    {
        agent.enabled = toggle;
    }

    public bool IsNavMeshAgentEnabled() => agent.enabled;

    public bool IsNavMeshEnabled() => agent.enabled;

    public bool IsNavMeshAgentStopped() => agent.isStopped;

    public bool CheckDestinationReached(Transform target)
    {
        float distToTarget = Vector3.Distance(transform.position, target.position);
        return distToTarget < destinationReachedThreshold;
    }

    public Animator ChangeAnimator(Animator anim)
    {

        animator = anim;

        return animator;
    }
}
