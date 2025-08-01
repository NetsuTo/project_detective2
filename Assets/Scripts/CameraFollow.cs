using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Player ���е��
    public Vector3 offset = new Vector3(0, 5, -10); // ������ҧ�ҡ Player
    public float smoothSpeed = 5f;    // ��������㹡�õ��

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target); // �ͧ价�� Player
    }
}
