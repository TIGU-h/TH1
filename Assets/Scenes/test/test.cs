using MalbersAnimations.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class test : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform firstCamera;

    private Animator animator;
    private float smoothVelocity;
    private CharacterController characterController;
    private Rigidbody _rigidbody;

    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float velocity;
    private bool canJump = true;
    private Vector3 direction;


    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();

    }
    private void Update()
    {
        MoveAndRotation();
        
        //ApplyGravity();

        //Debug.Log(Input.GetAxis("Jump").ToString());



        //if (Input.GetButton("Jump") && characterController.isGrounded && animator)
        //{

        //    animator.SetTrigger("jump");
        //    characterController.attachedRigidbody.velocity += Vector3.up * jumpForce;
        //    _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

    }

    public void JumpEnd()
    {
        canJump = true;
    }

    private void MoveAndRotation()
    {
        if (canJump)
        {

            float jump = Input.GetAxis("Jump")* jumpForce;
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            direction = new Vector3(horizontal, jump, vertical).normalized;

            if (direction.magnitude > 0f)
            {
                float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + firstCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
                characterController.Move(move.normalized * force * Time.deltaTime);
                if (jump == 0)
                    animator.SetBool("walk", true);
                else
                {
                    animator.SetTrigger("jump");
                    canJump = false;
                }
            }
            else if (direction.magnitude < 0f)
            {
                animator.SetBool("walk", true);
            }
            else
                animator.SetBool("walk", false);
        }
       

    }


    private void ApplyGravity()
    {
        if (characterController.isGrounded && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        characterController.Move(Vector3.down * -velocity * Time.deltaTime);
    }
}
