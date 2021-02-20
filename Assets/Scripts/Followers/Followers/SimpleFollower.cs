using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : Follower
{
    public bool CanBeUpgraded()
    {
        return followState == FollowState.Following;
    }
}
