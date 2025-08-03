[System.Serializable]
public class InventorySlotData
{
    public ItemData item;
    public int quantity;

    public InventorySlotData(ItemData item, int quantity = 1)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
