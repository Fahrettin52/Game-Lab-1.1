using UnityEngine;
using System.Collections;

public class CraftingBench : Inventory {

    public GameObject prefabButton;

    private GameObject previewSlot;

    public override void CreateLayout() {
        base.CreateLayout();

        GameObject craftButton;

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight + slotSize + slotPaddingTop);

        craftButton = Instantiate(prefabButton);

        RectTransform buttonRect = craftButton.GetComponent<RectTransform>();

        craftButton.name = "CraftButton";

        craftButton.transform.SetParent(this.transform.parent);

        buttonRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft, -slotPaddingTop * 4 - (slotSize * 3));

        buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((slotSize * 2) + slotPaddingLeft) * InventoryManager.Instance.canvas.scaleFactor);

        buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        craftButton.transform.SetParent(transform);


        previewSlot = Instantiate(InventoryManager.Instance.slotPrefab);

        RectTransform slotRect = previewSlot.GetComponent<RectTransform>();
         
        previewSlot.name = "PreviewSlot";

        previewSlot.transform.SetParent(this.transform.parent);

        slotRect.localPosition = inventoryRect.localPosition + new Vector3((slotPaddingLeft * 3) + (slotSize * 2), -slotPaddingTop * 4 - (slotSize * 3));

        slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        previewSlot.transform.SetParent(transform);
    }
}
