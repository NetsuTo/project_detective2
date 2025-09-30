using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillBarUI : MonoBehaviour
{
    public Transform skillSlotParent; // ��ͧʡ��� Canvas
    public GameObject skillSlotPrefab; // Prefab ���Ъ�ͧ (�� Text/Image)
    private List<GameObject> slots = new List<GameObject>();

    private PlayerSkillManager manager;

    private void Start()
    {
        manager = FindObjectOfType<PlayerSkillManager>();
        manager.onSkillUpdate += UpdateUI;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // ����ѧ�����ʡ����� ? ��͹ Panel
        if (manager.GetSkills().Count == 0)
        {
            skillSlotParent.gameObject.SetActive(false);
            return;
        }
        else
        {
            skillSlotParent.gameObject.SetActive(true);
        }

        // ��ҧ�ͧ���
        foreach (var slot in slots) Destroy(slot);
        slots.Clear();

        List<string> skills = manager.GetSkills();
        for (int i = 0; i < skills.Count; i++)
        {
            GameObject slot = Instantiate(skillSlotPrefab, skillSlotParent);
            slot.GetComponentInChildren<Text>().text = skills[i];

            if (i == manager.GetSelectedIndex())
                slot.GetComponent<Image>().color = Color.yellow; // highlight
            else
                slot.GetComponent<Image>().color = Color.white;

            slots.Add(slot);
        }
    }

}
