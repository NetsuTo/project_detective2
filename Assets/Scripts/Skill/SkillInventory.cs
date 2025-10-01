using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillInventory : MonoBehaviour
{
    public Transform bottleParent;   // �ش�ҧ UI � Canvas
    public GameObject bottlePrefab;  // Prefab icon/slot ����Ѻʡ�ż������
    public ElementMiniGameManager miniGameManager; // reference ��ѧ MiniGameManager (optional ����� global)

    private List<List<KeyCode>> storedSkills = new List<List<KeyCode>>();

    void Update()
    {
        // ��Ǩ��Ҽ����蹡� R
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (storedSkills.Count > 0)
            {
                Debug.Log("�� R ����! ������ʡ�Ũж١�Ѵ��ü�ҹ TargetZone");
            }
            else
            {
                Debug.Log("�����ʡ��㹢Ǵ�����");
            }
        }
    }

    // ����ʡ��������ҢǴ
    public void AddMixedSkill(List<KeyCode> sequence)
    {
        storedSkills.Add(sequence);

        GameObject go = Instantiate(bottlePrefab, bottleParent);
        Text t = go.GetComponentInChildren<Text>();
        if (t != null)
        {
            t.text = string.Join("", sequence); // �� HHO
        }
    }

    // ��Ǩ����� skill �ç�Ѻ seq �������
    public bool HasSkill(List<KeyCode> seq)
    {
        foreach (var s in storedSkills)
        {
            if (SequencesMatch(s, seq)) return true;
        }
        return false;
    }

    // �֧ sequence ���ç
    public List<KeyCode> GetSkillSequence(List<KeyCode> seq)
    {
        foreach (var s in storedSkills)
        {
            if (SequencesMatch(s, seq)) return new List<KeyCode>(s);
        }
        return null;
    }

    // źʡ�ŷ��ç�Ѻ sequence
    public void ConsumeSkill(List<KeyCode> seq)
    {
        for (int i = 0; i < storedSkills.Count; i++)
        {
            if (SequencesMatch(storedSkills[i], seq))
            {
                storedSkills.RemoveAt(i);
                if (i < bottleParent.childCount)
                {
                    Destroy(bottleParent.GetChild(i).gameObject);
                }
                return;
            }
        }
    }

    // źʡ���á�͡ (��Դ Zone ? ���¢Ǵ)
    public void ConsumeFirstSkill()
    {
        if (storedSkills.Count > 0)
        {
            storedSkills.RemoveAt(0);
            if (bottleParent.childCount > 0)
            {
                Destroy(bottleParent.GetChild(0).gameObject);
            }
        }
    }

    // ���Ǩ��������ʡ��� inventory
    public bool IsEmpty()
    {
        return storedSkills.Count == 0;
    }

    // ===== Helper =====

    public bool HasAnyBottle()
    {
        return storedSkills.Count > 0;
    }

    private bool SequencesMatch(List<KeyCode> a, List<KeyCode> b)
    {
        if (a == null || b == null) return false;
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    private string SeqToString(List<KeyCode> seq)
    {
        if (seq == null) return "";
        return string.Join("", seq);
    }
}
