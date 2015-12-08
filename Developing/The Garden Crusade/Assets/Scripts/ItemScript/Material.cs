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

    public override void Use(Slot slot, ItemScript item) {
    }
}