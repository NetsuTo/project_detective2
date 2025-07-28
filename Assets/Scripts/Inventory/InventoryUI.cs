using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public Transform slotParent;
    public GameObject slotPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void Refresh()
    {
        Debug.Log("Refresh UI: จำนวนช่อง = " + InventorySystem.instance.items.Count);

        // ล้างช่องเก่าก่อน
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // วนตามของทุกชิ้น ? แสดงทุกชิ้นแม้ชนิดเดียวกัน
        foreach (ItemData item in InventorySystem.instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<Image>().sprite = item.icon;

            // เคลียร์จำนวนถ้ามี
            Text text = slot.GetComponentInChildren<Text>();
            if (text != null) text.text = "";
        }
    }
}
