using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FighterBehavior))]
public class KnightFollower : Follower
{

    protected override void Start(){
        followBehavior = GetComponent<FighterBehavior>();
    }
}
