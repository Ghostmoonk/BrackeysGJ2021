using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : FollowerBehavior
{
    float initialStoppingDist;
    [SerializeField]
    float waitDurationAfterAttack = 0.5f;

    bool isAttacking;
    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }

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
            transform.LookAt(enemy.transform);
            animator.SetTrigger("Attack");
            isAttacking = true;
            StartCoroutine(StopMoveAfterAttack(enemy, damages, animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    public void SetNewStoppingDist(float newStoppingDist)
    {
        agent.stoppingDistance = newStoppingDist;
    }

    public void ResetStoppingDistance() => SetNewStoppingDist(initialStoppingDist);

    public IEnumerator StopMoveAfterAttack(Enemy enemy, int damages, float animDuration)
    {
        ToggleNavMesh(false);
        yield return new WaitForSeconds(animDuration / 2f);
        enemy.UpdateHealth(-damages);
        yield return new WaitForSeconds(animDuration / 2f + waitDurationAfterAttack);
        ToggleNavMesh(true);
        isAttacking = false;
        followerScript.UpdateHealth(-1);
    }
}
