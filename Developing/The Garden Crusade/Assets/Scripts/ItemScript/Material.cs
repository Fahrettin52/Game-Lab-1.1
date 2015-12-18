using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Material : Item {

    public Material(string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize)
        : base(itemName, description, itemType, quality, spriteNeutral, spriteHighlighted, maxSize) {
    }

    public Material() {
    }

    public override string GetToolTip(Inventory inv) {
        string materialTip = base.GetToolTip(inv);
        if (inv is VendorInventory) {
            return string.Format("{0} \n<size=20><color=yellow>Buy Price: {1} Crumbs</color></size>", materialTip, BuyPrice);
        } 
        else if (VendorInventory.Instance.IsOpen) {
        }
            return string.Format("{0} \n<size=20><color=yellow>Sell Price: {1} Crumbs</color></size>", materialTip, SellPrice);
        return materialTip;
    }

    public override void Use(Slot slot, ItemScript item) {
    }
}