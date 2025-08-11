using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            animator.SetBool("isJumping", false);
        }

        // Horizontal movement
        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x, 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(x));

        // Rotate to face left/right only
        if (x > 0.05f)
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);   // face right
        else if (x < -0.05f)
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);  // face left

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
