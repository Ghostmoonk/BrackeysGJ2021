using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : FollowerBehavior
{
    float initialStoppingDist;

    protected override void Start()
    {
        base.Start();
        initialStoppingDist = agent.stoppingDistance;
    }

    public void Attack(Enemy enemy, int damages)
    {
        //Do "physical" stuff like
        //Play attack animation, destroy the enemy then
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(StopMoveForTime(animator.GetCurrentAnimatorStateInfo(0).length));
        }
        enemy.UpdateHealth(-damages);
    }

    public void SetNewStoppingDist(float newStoppingDist)
    {
        agent.stoppingDistance = newStoppingDist;
    }

    public void ResetStoppingDistance() => SetNewStoppingDist(initialStoppingDist);

    public IEnumerator StopMoveForTime(float duration)
    {
        ToggleNavMesh(false);
        yield return new WaitForSeconds(duration);
        ToggleNavMesh(true);
    }
}
