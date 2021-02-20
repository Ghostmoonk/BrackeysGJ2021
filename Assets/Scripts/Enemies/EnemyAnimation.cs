using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAnimation : MonoBehaviour
{
    NavMeshAgent agent;

    Enemy enemyScript;

    Animator animator;

    private void Start()
    {
        enemyScript = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        
       

        

    }

    private void Update()
    {
        animator.SetFloat("Move", agent.velocity.magnitude);
        if (enemyScript.state.ToString() =="Attracting")
        {

            animator.SetBool("isDragging", true);
        }
    }
}
