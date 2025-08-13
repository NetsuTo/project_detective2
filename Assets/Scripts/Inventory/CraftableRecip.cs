using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/CraftRecipe")]
public class CraftableRecipe : ScriptableObject
{
    public ItemData[] ingredients; // ��ͧ�� 2 ������
    public ItemData result;
}
