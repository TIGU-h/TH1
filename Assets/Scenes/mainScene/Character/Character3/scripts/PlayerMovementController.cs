using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{

    //[SerializeField] private float walkSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform firstCamera;
    [SerializeField] private float gravityMultiplier = 3.0f;

    private Animator animator;
    private float smoothVelocity;
    private CharacterController characterController;

    private float gravity = -9.81f;
    private float velocity;
    private bool canJump = true;
    private bool canRotate = true;
    private bool forcedStop = false;






    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (forcedStop)
            return;
        ApplyGravity();

        animator.SetBool("isGrounded", characterController.isGrounded);

        if (canRotate)
            ApplyRotation();

        if (Input.GetButton("Jump") && characterController.isGrounded && canJump)
            Jump();

        if (Input.GetButton("Enable cursor"))
        {
            canRotate = false;
            //if(Cursor.lockState!=CursorLockMode.None)
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
        else
        {
            canRotate = true;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
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

    private void ApplyRotation()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetButton("Sprint");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude != 0f)
        {

            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + firstCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, rotSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            animator.SetBool("walk", true);
            animator.SetBool("run", sprint);
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
        }

    }

    private void Jump()
    {

        canRotate = false;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        animator.SetTrigger("jump");
        velocity = jumpForce;
    }

    public void ResetAnimatorParameters()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }

        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Trigger:
                    animator.ResetTrigger(parameter.name);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.name, false);
                    break;
            }
        }
    }





    //everything
    public void SetCanEverything()
    {
        canJump = true;
        canRotate = true;
        GetComponent<PlayerAttackAndSpellController>().SetCanAttack();

    }
    public void ResetCanEverything()
    {
        canJump = false;
        canRotate = false;
    }

    //canRotate
    public void SetCanRotate()
    {
        canRotate = true;
    }
    public void ResetCanRotate()
    {
        canRotate = false;
    }

    public void ForsedStop()
    {
        ResetAnimatorParameters();


        StartCoroutine(InvokeWithDelay(() =>
        {
            if (this != null)
                forcedStop = false;
        }, 0.2f));


    }




    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }

}
