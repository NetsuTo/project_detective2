using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // <<< เพิ่มบรรทัดนี้ เพื่อกำหนดไอเท็ม
    private bool playerInRange = false;
    private Transform player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventorySystem.instance.AddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
