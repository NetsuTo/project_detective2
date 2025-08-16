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

    private void Start()
    {
        // �������������������� �����Ҩ����⫹
        InventorySystem.instance.canUseItems = false;
        SetSlotsInteractable(false);
    }

    public void Refresh()
    {
        Debug.Log("Refresh UI: �ӹǹ��ͧ = " + InventorySystem.instance.items.Count);

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

    public void HighlightSelected(ItemData selected)
    {
        foreach (Transform t in slotParent)
        {
            var slot = t.GetComponent<InventorySlot>();
            if (slot != null) slot.SetSelected(slot.item == selected);
        }
    }

    public void SetSlotsInteractable(bool canUse)
    {
        foreach (Transform t in slotParent)
        {
            var btn = t.GetComponent<Button>();
            if (btn != null) btn.interactable = canUse;

            // ������ŵ��͡�����������
            if (!canUse)
            {
                var slot = t.GetComponent<InventorySlot>();
                if (slot != null) slot.SetSelected(false);
            }
        }
    }


}
