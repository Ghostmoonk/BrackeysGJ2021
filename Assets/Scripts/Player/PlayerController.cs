using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    #region Components
    PlayerMotor motor;
    #endregion

    #region Movements

    [SerializeField]
    float speed = 2.5f;

    [SerializeField]
    float rotationSpeed = 4f;


    [SerializeField]
    AnimationCurve AccelCurve;
    private float currentAccel;

    [SerializeField]
    AnimationCurve DecelCurve;
    private float currentDecel;


    Vector3 lastFrameVelocity;
    #endregion

    #region Animation

    Animator animator;
    #endregion

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();


    }

    private void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if (new Vector3(xMove, 0f, zMove).normalized != Vector3.zero)
        {
            currentAccel += Time.deltaTime;
            currentDecel -= Time.deltaTime;
        }
        else
        {
            currentAccel -= Time.deltaTime;
            currentDecel += Time.deltaTime;
        }
        currentAccel = Mathf.Clamp(currentAccel, 0f, AccelCurve[AccelCurve.length - 1].time);
        currentDecel = Mathf.Clamp(currentDecel, 0f, DecelCurve[DecelCurve.length - 1].time);

        Vector3 velocity = new Vector3(xMove, 0f, zMove).normalized * speed * AccelCurve.Evaluate(currentAccel);

        //If we stop moving
        if (velocity == Vector3.zero && lastFrameVelocity != Vector3.zero)
        {
            velocity = speed * DecelCurve.Evaluate(currentDecel) * lastFrameVelocity.normalized;
        }

        motor.Move(velocity);

        lastFrameVelocity = velocity;
        if (velocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed * Time.deltaTime);
            motor.Rotate(rotation);
        }

    }
}
