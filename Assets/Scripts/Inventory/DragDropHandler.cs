using UnityEngine;
using UnityEngine.UI;

public class DragDropHandler : MonoBehaviour
{
    public static DragDropHandler instance;

    public Image draggingIcon;   // ลาก DraggingItemImage มาใส่ (Raycast Target = OFF)
    public Canvas canvas;

    [HideInInspector] public ItemData draggedItem;
    [HideInInspector] public InventorySlot draggedFromSlot;

    void Awake()
    {
        instance = this;
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        if (draggingIcon != null) draggingIcon.gameObject.SetActive(false);
    }

    void Update()
    {
        if (draggingIcon != null && draggingIcon.gameObject.activeSelf)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var pos
            );
            draggingIcon.rectTransform.localPosition = pos;
        }
    }

    public void StartDrag(ItemData item, Sprite iconSprite, InventorySlot fromSlot)
    {
        draggedItem = item;
        draggedFromSlot = fromSlot;
        draggingIcon.sprite = iconSprite;
        draggingIcon.gameObject.SetActive(true);
    }

    public void EndDrag()
    {
        draggedItem = null;
        draggedFromSlot = null;
        if (draggingIcon != null) draggingIcon.gameObject.SetActive(false);
    }
}
