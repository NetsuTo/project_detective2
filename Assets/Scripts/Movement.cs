using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(inputX * moveSpeed, rb.linearVelocity.y, 0);
        rb.linearVelocity = move;
    }
}
