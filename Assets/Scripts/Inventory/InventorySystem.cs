using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [Header("Storage")]
    public List<ItemData> items = new List<ItemData>();
    public int maxSlots = 20;              // จำนวนช่องสูงสุด (1 ช่อง = 1 ชิ้น)

    [Header("Selection & Use")]
    public ItemData selectedItem;          // ไอเท็มที่ถูกเลือกอยู่ตอนนี้
    public bool canUseItems = false;       // true เมื่อผู้เล่นยืนใน UseZone (เปิดจาก PlayerController)

    private void Awake()
    {
        // ตั้ง Singleton แบบเรียบง่าย
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// เพิ่มไอเท็มเข้าอินเวนทอรี (1 ช่อง = 1 ชิ้น, ไม่ stack)
    /// </summary>
    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] AddItem: item เป็น null");
            return false;
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory เต็ม");
            return false;
        }

        items.Add(item);
        Debug.Log($"[InventorySystem] เก็บของ: {item.itemName} | คงเหลือ: {items.Count}");

        // อัปเดต UI
        if (InventoryUI.instance != null)
            InventoryUI.instance.Refresh();

        return true;
    }

    /// <summary>
    /// เลือกไอเท็ม (จะไม่อนุญาตถ้าอยู่นอก UseZone)
    /// </summary>
    public void SelectItem(ItemData item)
    {
        if (!canUseItems)
        {
            Debug.Log("อยู่นอก UseZone: ไม่อนุญาตให้เลือก/ใช้ไอเท็ม");
            return;
        }

        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] SelectItem: item เป็น null");
            return;
        }

        // ต้องเป็นชิ้นที่อยู่ในอินเวนทอรีจริง ๆ
        if (!items.Contains(item))
        {
            Debug.LogWarning("[InventorySystem] SelectItem: item ไม่ได้อยู่ในอินเวนทอรี");
            return;
        }

        selectedItem = item;
        Debug.Log($"[InventorySystem] เลือกไอเท็ม: {item.itemName}");

        // ไม่ Refresh UI ที่นี่ เพื่อรักษาไฮไลต์จาก InventorySlot
        // ถ้าคุณมีระบบไฮไลต์ผ่าน InventoryUI เอง ค่อยเรียกเมธอดไฮไลต์ที่นั่นแทน
    }

    /// <summary>
    /// ลบไอเท็ม "ชิ้นนั้น" ออกจากอินเวนทอรี 1 ชิ้น (ใช้เมื่อใช้สำเร็จ)
    /// </summary>
    public bool ConsumeOne(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] ConsumeOne: item เป็น null");
            return false;
        }

        int idx = items.IndexOf(item);
        if (idx >= 0)
        {
            items.RemoveAt(idx);
            if (selectedItem == item)
                selectedItem = null;

            if (InventoryUI.instance != null)
                InventoryUI.instance.Refresh();

            Debug.Log($"[InventorySystem] ใช้/ลบไอเท็ม: {item.itemName} | คงเหลือ: {items.Count}");
            return true;
        }

        Debug.LogWarning("[InventorySystem] ConsumeOne: ไม่พบ item ในอินเวนทอรี");
        return false;
    }

    /// <summary>
    /// ตรวจว่าอินเวนทอรีมีไอเท็ม "อินสแตนซ์นี้" หรือไม่
    /// </summary>
    public bool HasItem(ItemData item)
    {
        return item != null && items.Contains(item);
    }

    /// <summary>
    /// ลบไอเท็ม "อินสแตนซ์นี้" ออก (เหมือน ConsumeOne แต่ไม่เชื่อมโยงกับการใช้งาน)
    /// </summary>
    public bool RemoveItem(ItemData item)
    {
        if (item == null) return false;

        bool removed = items.Remove(item);
        if (removed)
        {
            if (selectedItem == item)
                selectedItem = null;

            if (InventoryUI.instance != null)
                InventoryUI.instance.Refresh();

            Debug.Log($"[InventorySystem] RemoveItem: {item.itemName}");
        }
        return removed;
    }

    /// <summary>
    /// เคลียร์การเลือกไอเท็ม (เช่น ตอนออกจากโซน)
    /// </summary>
    public void ClearSelected()
    {
        selectedItem = null;
        // ไม่จำเป็นต้อง Refresh UI เสมอไป เว้นแต่คุณต้องการล้างไฮไลต์ผ่านการสร้างช่องใหม่
    }
}
