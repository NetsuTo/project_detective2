using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemData itemData; // <<< ������÷Ѵ��� ���͡�˹������

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