using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/CraftRecipe")]
public class CraftableRecipe : ScriptableObject
{
    public ItemData[] ingredients; // ต้องมี 2 ชิ้นเป๊ะ
    public ItemData result;
}
