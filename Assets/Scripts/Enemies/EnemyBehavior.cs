using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Behavior of the enemy, controlled by the brain
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void GoToTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }

    public void Move(Vector3 direction)
    {
        agent.Move(direction);
    }

    public void StopDestination(bool _switch)
    {
        agent.isStopped = _switch;
    }

    public bool IsNavMeshAgentStopped() => agent.isStopped;


}
