using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    #region Components
    Rigidbody rb;
    #endregion

    #region Animation

    Animator animator;
    #endregion
    private Vector3 velocity;
    private Quaternion rotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Move", velocity.magnitude);
        
    }

    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
            

            //rb.velocity = velocity * Time.fixedDeltaTime;
            //transform.LookAt(transform.position + velocity);
            transform.rotation = rotation;
        }
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Quaternion _rotation)
    {
        rotation = _rotation;
    }
}