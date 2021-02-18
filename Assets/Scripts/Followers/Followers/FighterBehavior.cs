using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : FollowerBehavior
{
    protected override void Start()
    {
        base.Start();
    }

    public void Attack(Enemy enemy)
    {
        //Do "physical" stuff like
        //Play attack animation, destroy the enemy then
        Destroy(enemy.gameObject);
    }
}
