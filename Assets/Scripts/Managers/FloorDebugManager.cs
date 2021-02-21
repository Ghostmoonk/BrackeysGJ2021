using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDebugManager : MonoBehaviour
{
    [SerializeField]
    float floorY;

    private void Update()
    {
        foreach (Follower follower in GameObject.FindObjectsOfType<Follower>())
        {
            if (follower.transform.position.y < floorY)
            {
                follower.transform.position = new Vector3(
                    follower.transform.position.x,
                    floorY + follower.GetComponent<Collider>().bounds.extents.y,
                    follower.transform.position.z);

            }
        }
    }
}
