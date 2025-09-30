using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillManager : MonoBehaviour
{
    public List<string> skills = new List<string>(); // สกิลที่เก็บมา
    private int selectedSkillIndex = -1;

    public delegate void OnSkillUpdate();
    public event OnSkillUpdate onSkillUpdate;

    public bool CanPickupSkill(string skillID)
    {
        // ถ้ามีสกิลนั้นแล้ว ห้ามเก็บซ้ำ
        return !skills.Contains(skillID);
    }

    public void PickupSkill(string skillID)
    {
        if (CanPickupSkill(skillID))
        {
            skills.Add(skillID);
            if (selectedSkillIndex == -1) selectedSkillIndex = 0; // ถ้ายังไม่มี ให้เลือกอันแรกเลย
            onSkillUpdate?.Invoke();
            Debug.Log("Picked up skill: " + skillID);
        }
    }

    public void SelectNext()
    {
        if (skills.Count == 0) return;
        selectedSkillIndex = (selectedSkillIndex + 1) % skills.Count;
        onSkillUpdate?.Invoke();
    }

    public void SelectPrev()
    {
        if (skills.Count == 0) return;
        selectedSkillIndex = (selectedSkillIndex - 1 + skills.Count) % skills.Count;
        onSkillUpdate?.Invoke();
    }

    public string ConfirmSkill()
    {
        if (skills.Count == 0 || selectedSkillIndex < 0) return null;

        string skill = skills[selectedSkillIndex];
        Debug.Log("Confirmed skill: " + skill);

        // ตรวจสอบ SkillLetterSelector
        SkillLetterSelector selector = GetComponent<SkillLetterSelector>();
        if (selector == null)
        {
            Debug.LogError("ไม่มี SkillLetterSelector ติดอยู่บน Player!");
            return skill;
        }

        selector.StartSelection(skill);

        return skill;
    }



    public int GetSelectedIndex() => selectedSkillIndex;
    public List<string> GetSkills() => skills;
}
