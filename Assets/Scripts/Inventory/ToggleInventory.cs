using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    public GameObject inventoryPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // กด I เพื่อเปิด/ปิด
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
