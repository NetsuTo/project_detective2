using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("UI")]
    public Transform qteParent;           // Panel � Canvas
    public GameObject qteSlotPrefab;      // Prefab QTE Slot (Text + Image)
    public float slotSpacing = 60f;

    [Header("Timing Bar")]
    public GameObject timingBarPrefab;    // Prefab TimingBar
    private TimingBar currentTimingBar;

    [Header("Settings")]
    public float timePerSlot = 1f;

    private List<KeyCode> sequence = new List<KeyCode>();
    private List<GameObject> slotUIs = new List<GameObject>();
    private int currentIndex = 0;
    private bool isActive = false;

    // ���¡ spawn QTE slot ��ͷ���
    public void StartQTE(List<KeyCode> keySequence)
    {
        if (keySequence == null || keySequence.Count == 0)
            return;

        isActive = true;

        foreach (var key in keySequence)
        {
            SpawnQTESlot(key);
        }

        // spawn TimingBar ����á
        if (currentTimingBar == null && slotUIs.Count > 0)
        {
            SpawnTimingBar();
        }
    }

    void SpawnQTESlot(KeyCode key)
    {
        Vector2 startPos = Vector2.zero;
        if (slotUIs.Count > 0)
        {
            RectTransform lastRT = slotUIs[slotUIs.Count - 1].GetComponent<RectTransform>();
            startPos = lastRT.anchoredPosition + new Vector2(slotSpacing, 0f);
        }

        GameObject slot = Instantiate(qteSlotPrefab, qteParent);
        slot.transform.localScale = Vector3.one;

        RectTransform rt = slot.GetComponent<RectTransform>();
        rt.anchoredPosition = startPos;

        Text slotText = slot.GetComponentInChildren<Text>();
        if (slotText != null)
            slotText.text = key.ToString();

        Image img = slot.GetComponent<Image>();
        if (img != null)
            img.color = Color.white;

        slot.SetActive(false); // ��͹ slot ��͹
        slotUIs.Add(slot);     // Add ŧ list

        sequence.Add(key);
    }

    void SpawnTimingBar()
    {
        if (currentTimingBar != null)
            Destroy(currentTimingBar.gameObject);

        GameObject barGO = Instantiate(timingBarPrefab, qteParent.root); // Spawn �� Canvas
        currentTimingBar = barGO.GetComponent<TimingBar>();
        currentTimingBar.StartTiming(OnTimingComplete);
    }

    void OnTimingComplete(bool success)
    {
        if (!isActive) return;

        if (success)
        {
            if (currentIndex < slotUIs.Count)
                slotUIs[currentIndex].SetActive(true);

            // ? ����� SkillLetterSelector ź����ѡ�ô�ҹ�����
            SkillLetterSelector selector = FindObjectOfType<SkillLetterSelector>();
            if (selector != null)
            {
                selector.RemoveOneLetterUI();
            }

            currentIndex++;

            if (currentIndex >= sequence.Count)
            {
                Debug.Log("All QTE Success!");
                EndQTE();
            }
            else
            {
                SpawnTimingBar();
            }
        }
        else
        {
            Debug.Log("QTE Failed!");
            EndQTE();
        }
    }



    void EndQTE()
    {
        isActive = false;

        if (currentTimingBar != null)
            Destroy(currentTimingBar.gameObject);

        currentTimingBar = null;

        slotUIs.Clear();
        sequence.Clear();
        currentIndex = 0;
    }
}
