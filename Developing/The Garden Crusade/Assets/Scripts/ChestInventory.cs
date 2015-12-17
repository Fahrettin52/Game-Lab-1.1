using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChestInventory : Inventory {

    private List<Stack<ItemScript>> chestItems;
    private int chestSlots;
    public int maxSlot;

    public override void CreateLayout() {

        allSlots = new List<GameObject>();

        for (int i = 0; i < maxSlot; i++) {
            GameObject newSlot = Instantiate(InventoryManager.Instance.slotPrefab);
            newSlot.name = "Slot";
            newSlot.transform.SetParent(this.transform);
            allSlots.Add(newSlot);
            newSlot.GetComponent<Button>().onClick.AddListener
                (
                    delegate { MoveItem(newSlot); }
                );

            newSlot.SetActive(false);
        } 
    }

    public void UpdateLayOut (List<Stack<ItemScript>> items, int rows, int slots) {
        this.chestItems = items;
        this.chestSlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft; // calculates the width of the inventory

        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop; // calculates the height of the inventory

        inventoryRect = GetComponent<RectTransform>(); // creates a reference to the inventorys recttransform

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth); // sets the width of the inventory
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight); // sets the height of the inventory

        int columns = slots / rows; // calculates the amount of columns

        int index = 0;

        for (int y = 0; y < rows; y++) { // runs through the rows
            for (int x = 0; x < columns; x++) { // runs through the colums
                GameObject newSlot = allSlots[index];

                RectTransform slotRect = newSlot.GetComponent<RectTransform>(); // makes a reference to the rect transform

                newSlot.transform.SetParent(this.transform); // set the canvas as parent of the slot so that it is visible on the screen

                slotRect.localPosition = new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y)); // set the slot position

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor); // set the size of the slot
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor); // set the size of the slot

                newSlot.SetActive(true);

                index ++;
            }
        }
    }

    public void MoveItemsToChest() {
        chestItems.Clear();

        for (int i = 0; i < chestSlots; i++) {
            Slot tmpSlot = allSlots[i].GetComponent<Slot>();

            if (!tmpSlot.IsEmpty) {
                chestItems.Add(new Stack<ItemScript>(tmpSlot.Items));

                if (!IsOpen) {
                    tmpSlot.ClearSlot();
                }

            } else {
                chestItems.Add(new Stack<ItemScript>());
            }
            if (!IsOpen) {
                allSlots[i].SetActive(false);
            }
        }
    }

    protected override IEnumerator FadeOut() {
        yield return StartCoroutine(base.FadeOut());
        MoveItemsToChest();
    }
}
  