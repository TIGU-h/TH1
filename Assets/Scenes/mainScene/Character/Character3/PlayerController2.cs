using UnityEngine;

public class PlayerController2 : MonoBehaviour
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
    private bool canRotate = true;
    private bool canAttack = true;

    private bool firstAttack = true;



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




        if (canRotate)
            ApplyRotation();

        if (Input.GetButton("Jump") && characterController.isGrounded && canJump)
            Jump();

        if (Input.GetButtonDown("Fire1") && canAttack)
            Attack();




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
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref smoothVelocity, smoothTime);
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
        if (horizontal != 0 || vertical != 0)
        {
            ApplyRotation();
            animator.SetTrigger("jumpForward");
            velocity = jumpForce;

        }
        else
        {
            animator.SetTrigger("jump");
            velocity = jumpForce;

        }
        //animator.SetBool("walk", false);
        //animator.SetBool("run", false);




    }

    public void Attack()
    {
        //canAttack = false;
        if (firstAttack)
        {
            firstAttack = false;
            animator.SetTrigger("firstAttack");
        }
        else
        {
            animator.SetTrigger("nextAttack");
        }
    }



    //everything
    public void SetCanEverything()
    {
        canJump = true;
        canRotate = true;
        canAttack = true;
        firstAttack = true;

    }
    public void ResetCanEverything()
    {
        canJump = false;
        canRotate = false;
        canAttack = false;
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

    //CanNormalAttack
    public void SetCanAttack()
    {
        canAttack = true;
    }
    public void ResetNormal()
    {
        canAttack = false;
    }

    public void SetFirstAttack()
    {
        firstAttack = true;
    }

    public void WearponOn()
    {
        weapon.SetActive(true);
    }
    public void WeaponOff()
    {
        weapon.SetActive(false);
    }

}
