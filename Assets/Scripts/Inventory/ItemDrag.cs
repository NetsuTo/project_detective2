using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot slot;

    void Awake() { if (slot == null) slot = GetComponentInParent<InventorySlot>(); }

    public void OnBeginDrag(PointerEventData e)
    {
        if (slot != null && slot.item != null)
        {
            DragDropHandler.instance.StartDrag(slot.item, slot.icon.sprite, slot);
            Debug.Log("[ItemDrag] Begin: " + slot.item.itemName);
        }
    }

    public void OnDrag(PointerEventData e) { }

    public void OnEndDrag(PointerEventData e)
    {
        // ถ้า OnDrop ไม่ได้เกิด (ยังมีของค้างอยู่) ค่อยยกเลิก drag
        if (DragDropHandler.instance.draggedItem != null)
        {
            DragDropHandler.instance.EndDrag();
            Debug.Log("[ItemDrag] Cancel (no drop)");
        }
    }
}
