using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    #region Variables

    [SerializeField] private float MovementSpeed = 15.0f;

    [Header("Jump Settings")]
    [SerializeField] private float JumpForce = 1f;
    [SerializeField] private GameObject JumpCollider = null;
    [SerializeField] private bool isGrounded = true;

    private float Strafe = 0.0f;
    private float ForwardMovement = 0.0f;

    #endregion

    private void Update()
    {
        MovementInput();
        if(Input.GetButton("Jump")) { Jump(); }
    }

    private void Jump()
    {
        if(isGrounded && transform.GetComponent<Rigidbody>().velocity.y <= 0) { transform.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpForce, ForceMode.Impulse); }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Block>().getBlockData.getCollidable)
        {
            isGrounded = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Block>().getBlockData.getCollidable)
        {
            isGrounded = true;
        }
    }

    private void MovementInput()
    {
        Strafe = 0;
        ForwardMovement = 0;

        if (Input.GetAxis("Horizontal") != 0)
        {
            Strafe = Input.GetAxis("Horizontal") * MovementSpeed;
            Strafe *= Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") != 0)
        {
            ForwardMovement = Input.GetAxis("Vertical") * MovementSpeed;
            ForwardMovement *= Time.deltaTime;
        }
        transform.Translate(Strafe, 0, ForwardMovement);
    }

}
