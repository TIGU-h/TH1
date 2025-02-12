using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeedScale;
    [SerializeField] private float jumpForce;
    [SerializeField] private float smoothTime;
    [SerializeField] private Transform firstCamera;
    [SerializeField] private GameObject weapon;

    private Animator animator;
    private float smoothVelocity;
    private CharacterController characterController;

    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float velocity;
    private bool canJump = true;
    private bool canMove = true;
    private bool nowAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        ApplyGravity();

        animator.SetBool("isGrounded", characterController.isGrounded);
        nowAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        canJump = characterController.isGrounded && (!nowAttacking);

        canMove = !nowAttacking;


        if (canMove)
            MoveAndRotation();

        if (Input.GetButton("Jump") && animator.GetBool("isGrounded") && canJump)
            Jump();

        if (Input.GetButton("Fire1") && animator.GetBool("canNormalAttack") && !nowAttacking)
            Attack();




    }

    public void Attack()
    {
        SetNowAttacking();

        animator.SetTrigger("attack");
    }
    public void SetNowAttacking()
    {
        nowAttacking = true;
    }
    public void ResetNowAttacking()
    {
        nowAttacking = false;
    }
    public void SetCanNormalAttackInAnimator()
    {
        ResetNowAttacking();
        animator.SetBool("canNormalAttack", true);
    }
    public void ResetCanNormalAttackInAnimator()
    {
        animator.SetBool("canNormalAttack", false);
    }
    public void ResetIsJumping()
    {
        animator.SetBool("isJumping", false);

    }
    public void ResetCanMove()
    {
        canMove = false;

    }



    void Jump()
    {

        animator.SetTrigger("jump");
        animator.SetBool("isJumping", true);

        velocity = jumpForce;



    }
    private void MoveAndRotation()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetButton("Sprint");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude != 0f)
        {

            float rotationAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + firstCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 move = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
            characterController.Move(move.normalized * walkSpeed * (sprint ? runSpeedScale : 1) * Time.deltaTime);
            animator.SetBool("walk", true);
            animator.SetBool("run", sprint);



        }
        else
        {
            animator.SetBool("run", false);
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
