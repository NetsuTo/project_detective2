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

        // ��ҵ�ͧ�������ҧ��੾�Ъ�ͧ��ҧ
        if (currentItem != null)
        {
            Debug.Log("Slot occupied");
            dd.EndDrag();
            return;
        }

        // 1) ����͡�ҡ inventory
        bool removed = InventorySystem.instance.items.Remove(dd.draggedItem);
        if (!removed) Debug.LogWarning("Dragged item wasn't in inventory list");

        // 2) ���� craft slot
        SetItem(dd.draggedItem);

        // 3) ���ê��� UI �ͧ inventory (��ѧ�ҡ SetItem ���͡ѹ fromSlot �١ Destroy ���� Clear �����)
        InventoryUI.instance.Refresh();

        // 4) ������ҡ
        dd.EndDrag();
    }
}
