using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    public Image icon;
    public Image highlight;

    [Header("Data")]
    public ItemData item;

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        if (icon != null)
        {
            icon.enabled = item != null;
            icon.sprite = item != null ? item.icon : null;
        }
        SetSelected(false);
    }

    public void Clear() => SetItem(null);

    public void SetSelected(bool selected)
    {
        if (highlight != null) highlight.enabled = selected;
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (item == null) return;

        // อนุญาตก็ต่อเมื่ออยู่ในโซน (แฟล็กกลางถูกตั้งจาก PlayerController)
        if (!InventorySystem.instance.canUseItems)
        {
            Debug.Log("ต้องยืนอยู่ใน UseZone ก่อนถึงจะใช้ไอเท็มได้");
            return;
        }

        InventorySystem.instance.SelectItem(item);
        SetSelected(true);

        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) player.TryUseSelectedInZone();
    }



}
