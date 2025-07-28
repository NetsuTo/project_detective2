using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // <<< เพิ่มบรรทัดนี้ เพื่อกำหนดไอเท็ม

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventorySystem.instance.AddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }
}