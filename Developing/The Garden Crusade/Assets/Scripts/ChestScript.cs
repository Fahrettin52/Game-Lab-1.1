using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestScript : MonoBehaviour {

	public ChestInventory chestInventory;

    public int rows, slots;

    private List<Stack<ItemScript>> allSlots;

    void Start() {
        allSlots = new List<Stack<ItemScript>>();
    }

    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            chestInventory.UpdateLayOut(allSlots, rows, slots);
            //chestInventory.MoveItemsToChest();
        }
    }
}
 