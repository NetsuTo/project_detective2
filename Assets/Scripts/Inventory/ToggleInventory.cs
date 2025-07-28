using UnityEngine;

public class ToggleInventory : MonoBehaviour
{
    public GameObject inventoryPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // �� I �����Դ/�Դ
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
