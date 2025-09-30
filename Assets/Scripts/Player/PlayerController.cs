using System.Collections.Generic;
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

    // ===== Use Zone (�ͧ�Ѻ����⫹) =====
    [Header("Use Zone")]
    public UseZone currentUseZone;                 // ⫹�����ҹ����
    private readonly HashSet<UseZone> zonesIn = new HashSet<UseZone>(); // ⫹�����������ѧ�׹����

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // ? �᡹��ҵ͹ spawn �׹�����⫹������������
        ScanZonesAtStart();
    }

    private void ScanZonesAtStart()
    {
        zonesIn.Clear();

        var allZones = FindObjectsOfType<UseZone>(true);
        Vector3 p = transform.position;
        foreach (var z in allZones)
        {
            var col = z.GetComponent<Collider>();
            if (col == null) continue;

            // ��� player ����� bounds �ͧ⫹���������� ? ������Ҫش
            if (col.bounds.Contains(p))
                zonesIn.Add(z);
        }

        RecomputeCurrentZone(); // ���繤��Դ/�Դ canUseItems ��� UI ����ͧ
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

        // Horizontal movement (᡹ X)
        float x = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(x, 0f, 0f);
        controller.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(x));

        // �ѹ����/���
        if (x > 0.05f) transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        else if (x < -0.05f) transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // �����˵�: ��� "�������" ���¡�ҡ InventorySlot ? player.TryUseSelectedInZone()
    }

    // ========== ���͡⫹���շ���ش �����������⫹ ==========
    private void RecomputeCurrentZone()
    {
        UseZone best = null;
        float bestDist = float.MaxValue;
        int bestPriority = int.MinValue;

        Vector3 p = transform.position;

        foreach (var z in zonesIn)
        {
            if (z == null) continue;
            var col = z.GetComponent<Collider>();
            if (col == null) continue;

            // ���͡��� priority ��͹
            if (z.priority > bestPriority)
            {
                best = z;
                bestPriority = z.priority;
                bestDist = Vector3.SqrMagnitude(col.bounds.ClosestPoint(p) - p);
                continue;
            }
            if (z.priority < bestPriority) continue;

            // priority ��ҡѹ ? ���͡�ѹ���������
            float d = Vector3.SqrMagnitude(col.bounds.ClosestPoint(p) - p);
            if (d < bestDist)
            {
                best = z;
                bestDist = d;
            }
        }

        currentUseZone = best;

        bool inAnyZone = currentUseZone != null;
    }

    // ========== ��������Ѻ⫹�Ѩ�غѹ ==========
    public void TryUseSelectedInZone()
    {
        if (currentUseZone == null)
        {
            Debug.Log("�ѧ������׹�����⫹��ҹ");
            return;
        }
    }

    // ========== Trigger: ���/�͡����⫹�������ѹ ==========
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UseZone zone))
        {
            zonesIn.Add(zone);
            RecomputeCurrentZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UseZone zone))
        {
            zonesIn.Remove(zone);
            RecomputeCurrentZone();
        }
    }
}
