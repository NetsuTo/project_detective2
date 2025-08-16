using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [Header("Storage")]
    public List<ItemData> items = new List<ItemData>();
    public int maxSlots = 20;              // �ӹǹ��ͧ�٧�ش (1 ��ͧ = 1 ���)

    [Header("Selection & Use")]
    public ItemData selectedItem;          // ��������١���͡����͹���
    public bool canUseItems = false;       // true ����ͼ������׹� UseZone (�Դ�ҡ PlayerController)

    private void Awake()
    {
        // ��� Singleton Ẻ���º����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// �������������Թ�ǹ���� (1 ��ͧ = 1 ���, ��� stack)
    /// </summary>
    public bool AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] AddItem: item �� null");
            return false;
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory ���");
            return false;
        }

        items.Add(item);
        Debug.Log($"[InventorySystem] �红ͧ: {item.itemName} | �������: {items.Count}");

        // �ѻവ UI
        if (InventoryUI.instance != null)
            InventoryUI.instance.Refresh();

        return true;
    }

    /// <summary>
    /// ���͡����� (�����͹حҵ�������͡ UseZone)
    /// </summary>
    public void SelectItem(ItemData item)
    {
        if (!canUseItems)
        {
            Debug.Log("����͡ UseZone: ���͹حҵ������͡/�������");
            return;
        }

        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] SelectItem: item �� null");
            return;
        }

        // ��ͧ�繪�鹷��������Թ�ǹ���ը�ԧ �
        if (!items.Contains(item))
        {
            Debug.LogWarning("[InventorySystem] SelectItem: item �����������Թ�ǹ����");
            return;
        }

        selectedItem = item;
        Debug.Log($"[InventorySystem] ���͡�����: {item.itemName}");

        // ��� Refresh UI ����� �����ѡ�����ŵ�ҡ InventorySlot
        // ��Ҥس���к����ŵ��ҹ InventoryUI �ͧ �������¡���ʹ���ŵ�����᷹
    }

    /// <summary>
    /// ź����� "��鹹��" �͡�ҡ�Թ�ǹ���� 1 ��� (��������������)
    /// </summary>
    public bool ConsumeOne(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("[InventorySystem] ConsumeOne: item �� null");
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

            Debug.Log($"[InventorySystem] ��/ź�����: {item.itemName} | �������: {items.Count}");
            return true;
        }

        Debug.LogWarning("[InventorySystem] ConsumeOne: ��辺 item ��Թ�ǹ����");
        return false;
    }

    /// <summary>
    /// ��Ǩ����Թ�ǹ����������� "�Թ�ᵹ����" �������
    /// </summary>
    public bool HasItem(ItemData item)
    {
        return item != null && items.Contains(item);
    }

    /// <summary>
    /// ź����� "�Թ�ᵹ����" �͡ (����͹ ConsumeOne �����������§�Ѻ�����ҹ)
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
    /// �����������͡����� (�� �͹�͡�ҡ⫹)
    /// </summary>
    public void ClearSelected()
    {
        selectedItem = null;
        // �����繵�ͧ Refresh UI ����� �����س��ͧ�����ҧ���ŵ��ҹ������ҧ��ͧ����
    }
}
