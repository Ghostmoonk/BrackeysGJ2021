using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    #region Components
    Camera cam;
    #endregion

    #region Follow
    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 offset;

    [Range(0f, 1f)]
    [SerializeField]
    float smoothness;
    #endregion

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        Vector3 finalPosition = target.position + offset;
        transform.position = finalPosition;
        transform.LookAt(target);
    }
    private void Update()
    {
        FollowTarget();
    }

    //Follow camera to target keeping offset
    void FollowTarget()
    {
        Vector3 finalPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, finalPosition, smoothness);
    }
}
