// ElementMiniGameManager.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ElementMiniGameManager : MonoBehaviour
{
    [Header("UI �ͧ MiniGame � Zone ���")]
    public Text displayText;                 // �ʴ� key �Ѩ�غѹ (UI Text)
    public GameObject failSymbol;            // �ͤ͹/UI �ʴ� Fail
    public float failSymbolDuration = 5f;    // ���ҷ�� Fail symbol ����������

    [Header("Optional: ��� StartMiniGame �١���¡���� null, �� fallback ���� inspectorSequence")]
    public List<KeyCode> inspectorSequence = new List<KeyCode>();

    [Header("Events (���� Inspector)")]
    public UnityEvent onSuccessEvent;        // ���¡�͹�Թ��������
    public UnityEvent onFailEvent;           // ���¡�͹�Թ���������� (�͡�˹�ͨҡ FailSymbol)

    // internal
    private List<KeyCode> currentSequence = new List<KeyCode>();
    private int currentIndex = 0;
    private bool isActive = false;
    private Action<bool> onCompleteCallback = null;

    void Start()
    {
        if (displayText != null) displayText.gameObject.SetActive(false);
        if (failSymbol != null) failSymbol.SetActive(false);
    }

    void Update()
    {
        if (!isActive) return;
        if (currentSequence == null || currentSequence.Count == 0) return;

        // ��Ǩ input: ����� key �� �������ҵç�Ѻ key �Ѩ�غѹ�������
        if (Input.anyKeyDown)
        {
            // �� key �١��ͧ
            if (Input.GetKeyDown(currentSequence[currentIndex]))
            {
                currentIndex++;
                UpdateDisplay();

                if (currentIndex >= currentSequence.Count)
                    Success();
            }
            else
            {
                // key �Դ
                Fail();
            }
        }
    }

    /// <summary>
    /// ������Թ������� sequence ���������� (��� sequence �� null ������ҧ �� fallback 价�� inspectorSequence �����)
    /// callback �١���¡������Թ����� (true=success, false=fail)
    /// </summary>
    public void StartMiniGame(List<KeyCode> sequence, Action<bool> callback)
    {
        // ��� sequence ������¡��ҧ ��� fallback ��� inspectorSequence (�����)
        if (sequence == null || sequence.Count == 0)
        {
            if (inspectorSequence != null && inspectorSequence.Count > 0)
            {
                currentSequence = new List<KeyCode>(inspectorSequence);
            }
            else
            {
                Debug.LogWarning("[ElementMiniGameManager] StartMiniGame called with empty sequence AND no inspectorSequence to fallback.");
                callback?.Invoke(false);
                return;
            }
        }
        else
        {
            currentSequence = new List<KeyCode>(sequence);
        }

        onCompleteCallback = callback;
        currentIndex = 0;
        isActive = true;

        if (displayText != null)
        {
            displayText.gameObject.SetActive(true);
            UpdateDisplay();
        }

        if (failSymbol != null)
            failSymbol.SetActive(false);

        Debug.Log($"[ElementMiniGameManager:{gameObject.name}] StartMiniGame - seq: {SeqToString(currentSequence)}");
    }

    private void UpdateDisplay()
    {
        if (displayText == null) return;

        if (currentIndex < currentSequence.Count)
            displayText.text = "Next: " + KeyToSymbol(currentSequence[currentIndex]);
        else
            displayText.text = "Done!";
    }

    private void Success()
    {
        isActive = false;
        if (displayText != null) displayText.gameObject.SetActive(false);

        Debug.Log("[ElementMiniGameManager] MiniGame Success in " + gameObject.name);

        // ���¡ UnityEvent ��������� Inspector
        try { onSuccessEvent?.Invoke(); } catch (Exception ex) { Debug.LogWarning("onSuccessEvent invoke failed: " + ex); }

        // callback ��� caller (�� TargetZone / SkillInventory)
        onCompleteCallback?.Invoke(true);

        // reset callback (��ͧ�ѹ������¡���)
        onCompleteCallback = null;
    }

    private void Fail()
    {
        isActive = false;
        if (displayText != null) displayText.gameObject.SetActive(false);

        Debug.Log("[ElementMiniGameManager] MiniGame Fail in " + gameObject.name);

        // �ʴ� Fail symbol ��� hide �ѵ��ѵ�
        ShowFailSymbolSafe();

        // ���¡ UnityEvent ����Ѻ Fail (optional)
        try { onFailEvent?.Invoke(); } catch (Exception ex) { Debug.LogWarning("onFailEvent invoke failed: " + ex); }

        onCompleteCallback?.Invoke(false);
        onCompleteCallback = null;
    }

    /// <summary>
    /// �ʴ� Fail symbol (��ʹ��� ���¡�����) � �Ы�͹�ѵ��ѵ���ѧ failSymbolDuration
    /// </summary>
    public void ShowFailSymbolSafe()
    {
        if (failSymbol == null) return;
        StopAllCoroutines();
        StartCoroutine(ShowFailSymbolCoroutine());
    }

    private IEnumerator ShowFailSymbolCoroutine()
    {
        failSymbol.SetActive(true);
        yield return new WaitForSeconds(failSymbolDuration);
        failSymbol.SetActive(false);
    }

    // �ŧ KeyCode �繵���ѡ��/�ѭ�ѡɳ�����Ѻ�ʴ� (customize ��)
    private string KeyToSymbol(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.UpArrow: return "?";
            case KeyCode.DownArrow: return "?";
            case KeyCode.LeftArrow: return "?";
            case KeyCode.RightArrow: return "?";
            case KeyCode.Space: return "Space";
            default: return key.ToString(); // e.g. A, H, O ...
        }
    }

    private string SeqToString(List<KeyCode> seq)
    {
        if (seq == null || seq.Count == 0) return "";
        return string.Join("", seq);
    }
}
