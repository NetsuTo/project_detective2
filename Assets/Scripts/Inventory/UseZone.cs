using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UseZone : MonoBehaviour
{
    [Header("Info")]
    public string zoneName = "Use Zone";
    public int priority = 0; // มากกว่า = สำคัญกว่าเวลาซ้อนกัน

    [Header("Rules")]
    public List<ItemData> acceptableItems = new List<ItemData>();

    [Header("Events")]
    public UnityEvent onUsed;

    public bool CanUse(ItemData item) => item != null && acceptableItems.Contains(item);

    public bool TryUse(ItemData item)
    {
        if (!CanUse(item)) return false;
        onUsed?.Invoke();
        return true;
    }

    // (ทางเลือก) ให้เห็นใน Scene ว่าโซนชื่ออะไร
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var col = GetComponent<Collider>();
        if (col != null) Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
        var style = new GUIStyle { normal = { textColor = Color.cyan } };
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, $"{zoneName} (P:{priority})", style);
#endif
    }
}
