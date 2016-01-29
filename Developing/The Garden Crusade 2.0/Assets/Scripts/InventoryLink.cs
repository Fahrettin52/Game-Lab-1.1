using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryLink : MonoBehaviour {

	public ChestInventory linkedInventory;

    public int rows, slots;

    private List<Stack<ItemScript>> allSlots;

    public GameObject soundToOpenShop;

    void Start() {
        linkedInventory = GameObject.Find("VendorBackground").GetComponent<VendorInventory>();
        allSlots = new List<Stack<ItemScript>>();
    }

    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            if (linkedInventory.FadingOut) {
                linkedInventory.instantClose = true;
                linkedInventory.MoveItemsToChest();
            }
            linkedInventory.UpdateLayOut(allSlots, rows, slots);
        }
    }
    public void ShopSound()
    {
        Instantiate(soundToOpenShop, transform.position, transform.rotation);
    }
}
 