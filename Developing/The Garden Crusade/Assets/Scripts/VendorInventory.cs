using UnityEngine;
using System.Collections;

public class VendorInventory : ChestInventory {
    protected override void Start() {
        EmptySlots = slots;
        base.Start();   
    }
}
