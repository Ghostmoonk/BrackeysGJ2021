using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollower : Follower
{

    public bool CanBeUpgraded() => followState == FollowState.Following;

    public bool CanBeAttracted() => followState == FollowState.Following;

}