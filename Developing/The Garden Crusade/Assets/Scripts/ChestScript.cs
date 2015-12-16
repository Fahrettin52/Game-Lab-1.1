using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestScript : MonoBehaviour {

	public ChestInventory chestInventory;

    public int row, slots, slotSize;

    private List<Stack<ItemScript>> allSlots;

    void Start() {
        allSlots = new List<Stack<ItemScript>>();
    }

}
 