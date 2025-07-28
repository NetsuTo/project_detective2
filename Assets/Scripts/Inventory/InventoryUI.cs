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
        Debug.Log("Refresh UI: �ӹǹ��ͧ = " + InventorySystem.instance.items.Count);

        // ��ҧ��ͧ��ҡ�͹
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        // ǹ����ͧ�ء��� ? �ʴ��ء�����骹Դ���ǡѹ
        foreach (ItemData item in InventorySystem.instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.GetComponent<Image>().sprite = item.icon;

            // ������ӹǹ�����
            Text text = slot.GetComponentInChildren<Text>();
            if (text != null) text.text = "";
        }
    }
}
