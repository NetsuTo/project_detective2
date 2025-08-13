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

        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        foreach (ItemData item in InventorySystem.instance.items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotParent);
            var invSlot = slotGO.GetComponent<InventorySlot>();
            if (invSlot != null)
            {
                invSlot.SetItem(item);
            }
        }
    }

}
