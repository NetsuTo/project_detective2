using System.Linq;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public CraftSlot[] craftSlots;
    public CraftableRecipe[] recipes;

    public void TryCraft()
    {
        var inputItems = craftSlots
            .Where(slot => slot.currentItem != null)
            .Select(slot => slot.currentItem)
            .ToArray();

        foreach (var recipe in recipes)
        {
            if (MatchRecipe(recipe, inputItems))
            {
                InventorySystem.instance.AddItem(recipe.result);
                Debug.Log($"Crafted: {recipe.result.itemName}");

                foreach (var slot in craftSlots) slot.Clear();
                return;
            }
        }

        Debug.Log("ไม่ตรงกับสูตรใดเลย");
    }

    bool MatchRecipe(CraftableRecipe recipe, ItemData[] inputs)
    {
        if (recipe.ingredients.Length != inputs.Length) return false;

        return recipe.ingredients.All(i => inputs.Contains(i))
            && inputs.All(i => recipe.ingredients.Contains(i));
    }
}
