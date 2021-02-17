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

    #endregion

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 velocity = new Vector3(xMove, 0f, zMove).normalized * speed;
        motor.Move(velocity);

        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity, Vector3.up), rotationSpeed * Time.deltaTime);
        motor.Rotate(rotation);
    }
}
