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
            print(currentAccel);
        }
        else
        {
            currentAccel -= Time.deltaTime;
        }
        currentAccel = Mathf.Clamp(currentAccel, 0f,1f);

        

        Vector3 velocity = new Vector3(xMove, 0f, zMove).normalized * speed * AccelCurve.Evaluate(currentAccel);
        motor.Move(velocity);


        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed * Time.deltaTime);
        motor.Rotate(rotation);


        



    }
}
