using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingBench : Inventory {

    public GameObject prefabButton;
    private GameObject previewSlot;
    private Dictionary<string, Item> craftingItems = new Dictionary<string, Item>();

    public override void CreateLayout() {
        base.CreateLayout();

        GameObject craftBtn;

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight + slotSize + slotPaddingTop);

        craftBtn = Instantiate(prefabButton);

        RectTransform buttonRect = craftBtn.GetComponent<RectTransform>();

        craftBtn.name = "CraftButton";

        craftBtn.transform.SetParent(this.transform.parent);

        buttonRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft, -slotPaddingTop * 4 - (slotSize * 3));

        buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ((slotSize * 2) + slotPaddingLeft) * InventoryManager.Instance.canvas.scaleFactor);

        buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        craftBtn.transform.SetParent(transform);

        craftBtn.GetComponent<Button>().onClick.AddListener(CraftItem);


        previewSlot = Instantiate(InventoryManager.Instance.slotPrefab);

        RectTransform slotRect = previewSlot.GetComponent<RectTransform>();
         
        previewSlot.name = "PreviewSlot";

        previewSlot.transform.SetParent(this.transform.parent);

        slotRect.localPosition = inventoryRect.localPosition + new Vector3((slotPaddingLeft * 3) + (slotSize * 2), -slotPaddingTop * 4 - (slotSize * 3));

        slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor);

        previewSlot.transform.SetParent(transform);
    }

    public void CreateBlueprints() {
        craftingItems.Add("EMPTY-Iron-EMPTY-EMPTY-Iron-EMPTY-EMPTY-Wood-EMPTY-", InventoryManager.Instance.ItemContainer.Equipment.Find(x => x.ItemName == "Wooden Sword"));
    }

    public void CraftItem() {
        string output = string.Empty;

        foreach (GameObject slot in allSlots) {  
            Slot tmp = slot.GetComponent<Slot>();
            if (tmp.IsEmpty) {
                output += "EMPTY-";
            }
            else {
                output += tmp.CurrentItem.Item.ItemName + "-";
            }
        }
        Debug.Log(output);
    }
}
