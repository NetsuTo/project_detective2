using UnityEngine;
using System.Collections.Generic;

public class TargetZone : MonoBehaviour
{
    [Header("MiniGame �ͧ Zone ��� (Drag Component ������١�����)")]
    public ElementMiniGameManager miniGame;   // ���ҡ Inspector

    [Header("Sequence ����ͧ��èҡ�Ǵ (�� HHO, O2, HOH)")]
    public List<KeyCode> requiredSequence = new List<KeyCode>();

    private bool playerInside = false;
    private SkillInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerInventory = other.GetComponent<SkillInventory>();
            Debug.Log("Player entered TargetZone (need: " + SeqToString(requiredSequence) + ")");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerInventory = null;
            Debug.Log("Player left TargetZone");
        }
    }

    private void Update()
    {
        if (!playerInside) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerInventory == null || playerInventory.IsEmpty())
            {
                Debug.Log("? �����ʡ��㹢Ǵ�����");
                return;
            }

            if (playerInventory.HasSkill(requiredSequence))
            {
                Debug.Log("?? ��ʡ�ŵç�Ѻ Zone, ����� MiniGame (�� inspectorSequence)");

                // ź�Ǵ�͡仡�͹������Թ���
                playerInventory.ConsumeSkill(requiredSequence);

                // ? ���¡ MiniGame ������� sequence ? ���� inspectorSequence �ͧ Zone
                miniGame.StartMiniGame(null, (success) =>
                {
                    if (success)
                    {
                        Debug.Log("?? Success MiniGame in Zone: " + SeqToString(requiredSequence));
                        // trigger event �ͧ Zone ������
                    }
                    else
                    {
                        Debug.Log("?? Fail MiniGame in Zone: " + SeqToString(requiredSequence));
                        miniGame.ShowFailSymbolSafe();
                    }
                });
            }
            else
            {
                Debug.Log("? ����� skill ���١��ͧ����Ѻ Zone ���");
                miniGame.ShowFailSymbolSafe();
                playerInventory.ConsumeFirstSkill(); // �Ǵ��衴�Դ�����
            }
        }
    }

    private string SeqToString(List<KeyCode> seq)
    {
        if (seq == null) return "";
        return string.Join("", seq);
    }
}
