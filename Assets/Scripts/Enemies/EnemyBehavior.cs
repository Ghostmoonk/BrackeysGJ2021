using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Behavior of the enemy, controlled by the brain
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    float iniSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        iniSpeed = agent.speed;
    }

    public void GoToTarget(Transform target)
    {
        agent.SetDestination(target.position);
        transform.LookAt(target);
    }

    public void GoToTarget(Vector3 target)
    {
        agent.SetDestination(target);
        transform.LookAt(target);
    }

    public void SetNewSpeed(float newSpeed) => agent.speed = newSpeed;

    public void ResetSpeed() => agent.speed = iniSpeed;

    public void Move(Vector3 direction)
    {
        agent.Move(direction);
    }

    public void MoveVelocity(Vector3 moveForce)
    {
        agent.velocity = moveForce;
        transform.LookAt(transform.position + moveForce * 100f);
    }

    public void StopDestination(bool _switch)
    {
        agent.isStopped = _switch;
    }

    public bool IsNavMeshAgentStopped() => agent.isStopped;


}
