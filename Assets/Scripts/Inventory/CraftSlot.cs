using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftSlot : MonoBehaviour, IDropHandler
{
    public ItemData currentItem;
    public Image icon;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (icon != null)
        {
            icon.enabled = (item != null);
            icon.sprite = item != null ? item.icon : null;
        }
    }

    public void Clear() => SetItem(null);

    public void OnDrop(PointerEventData e)
    {
        Debug.Log("[CraftSlot] OnDrop");
        var dd = DragDropHandler.instance;
        if (dd == null || dd.draggedItem == null) { Debug.Log("No dragged item"); return; }

        // ถ้าต้องการให้วางได้เฉพาะช่องว่าง
        if (currentItem != null)
        {
            Debug.Log("Slot occupied");
            dd.EndDrag();
            return;
        }

        // 1) เอาออกจาก inventory
        bool removed = InventorySystem.instance.items.Remove(dd.draggedItem);
        if (!removed) Debug.LogWarning("Dragged item wasn't in inventory list");

        // 2) ใส่ใน craft slot
        SetItem(dd.draggedItem);

        // 3) รีเฟรชฝั่ง UI ของ inventory (หลังจาก SetItem เพื่อกัน fromSlot ถูก Destroy แล้ว Clear ไม่ได้)
        InventoryUI.instance.Refresh();

        // 4) จบการลาก
        dd.EndDrag();
    }
}
