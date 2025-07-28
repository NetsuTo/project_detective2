using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public List<ItemData> items = new List<ItemData>();
    public int maxSlots = 20;

    private void Awake()
    {
        instance = this;
    }

    public bool AddItem(ItemData item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory ���");
            return false;
        }

        items.Add(item);
        Debug.Log("�红ͧ: " + item.itemName + " | �������: " + items.Count);
        InventoryUI.instance.Refresh();
        return true;
    }
}

