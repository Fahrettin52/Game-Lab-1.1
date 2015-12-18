using UnityEngine;
using System.Collections;

public class VendorInventory : ChestInventory {
    protected override void Start() {
        EmptySlots = slots;
        base.Start();
        GiveItem("Health Potion");
        GiveItem("Wooden Sword");
        GiveItem("Leather Boots");
    }

    protected void GiveItem (string itemName) {
        GameObject tmp = Instantiate(InventoryManager.Instance.itemObject);
        tmp.AddComponent<ItemScript>();
        ItemScript newItem = tmp.GetComponent<ItemScript>();

        if (InventoryManager.Instance.ItemContainer.Consumable.Exists(x => x.ItemName == itemName)) {
            newItem.Item = InventoryManager.Instance.ItemContainer.Consumable.Find(x => x.ItemName == itemName);
        }
        else if (InventoryManager.Instance.ItemContainer.Weapons.Exists(x => x.ItemName == itemName)) {
            newItem.Item = InventoryManager.Instance.ItemContainer.Weapons.Find(x => x.ItemName == itemName);
        }
        else if (InventoryManager.Instance.ItemContainer.Equipment.Exists(x => x.ItemName == itemName)) {
            newItem.Item = InventoryManager.Instance.ItemContainer.Equipment.Find(x => x.ItemName == itemName);
        }

        if (newItem != null) {
            AddItem(newItem);
        }

        Destroy(tmp);
    }
}
