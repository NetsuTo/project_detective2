using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;    // �ҡ Image �ͧ "Icon" �١�����
    public ItemData item;

    public void SetItem(ItemData newItem)
    {
        item = newItem;
        if (icon != null)
        {
            if (item != null && item.icon != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }
    }

    public void Clear()
    {
        SetItem(null);
    }
}
