using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        }
    }
}
 