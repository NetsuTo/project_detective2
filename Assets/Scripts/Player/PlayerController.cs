using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float climbSpeed = 4f;
    public LayerMask groundLayer;

    [Header("Death & Respawn")]
    public float fallThresholdY = -10f;
    public Transform respawnPoint;
    public float respawnDelay = 1f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canClimb = false; // ผู้เล่นอยู่ในเขตปีนได้ไหม
    private bool isClimbing = false; // ผู้เล่นกำลังปีนอยู่ไหม
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        CheckGrounded();

        float moveInput = Input.GetAxis("Horizontal");

        if (isClimbing)
        {
            float climbInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(0, climbInput * climbSpeed, 0);
        }
        else
        {
            Vector3 move = new Vector3(moveInput * moveSpeed, rb.velocity.y, 0);
            rb.velocity = move;
        }

        if (transform.position.y < fallThresholdY)
        {
            Die();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isClimbing)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        if (Input.GetKeyDown(KeyCode.F) && canClimb)
        {
            ToggleClimb();
        }
    }

    private void ToggleClimb()
    {
        isClimbing = !isClimbing;

        if (isClimbing)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, 1.1f, groundLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = true;
            Debug.Log("เข้าสู่เขตปีนได้");
        }
        else if (other.CompareTag("Trap"))
        {
            Die();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            canClimb = false;

            if (isClimbing)
            {
                ToggleClimb(); // ถ้าเดินออกจากบันไดขณะปีนอยู่ ให้หยุดปีนทันที
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        Debug.Log("Player Died");

        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        transform.position = respawnPoint.position;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        isDead = false;
    }
}
