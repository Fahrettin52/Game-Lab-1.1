using UnityEngine;
using System.Collections;

public class Weapon : Equipment {

	public float AttackSpeed { get; set; }

	public Weapon () {
	}
	
	public Weapon (string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int intellect, int agility, int stamina, int strength, float attackSpeed) 
		: base(itemName,description,itemType,quality,spriteNeutral,spriteHighlighted,maxSize,intellect,agility,stamina,strength) {
		this.AttackSpeed = attackSpeed; 
	}

    public override void Use(Slot slot, ItemScript item)
    {
        CharacterPanel.Instance.EquipItem(slot, item);
    }

    public override string GetToolTip (Inventory inv) {

        string equipmentTip = base.GetToolTip(inv);

        if (inv is VendorInventory) {
            //Adds the attackspeed to the tooltip
            return string.Format("{0} \n <size=20>AttackSpeed: {1}\n<color=yellow>Price: {2}</color></size>", equipmentTip, AttackSpeed, BuyPrice);
        } else if (VendorInventory.Instance.IsOpen) {
            //Adds the attackspeed to the tooltip
            return string.Format("{0} \n <size=20>AttackSpeed: {1}\n<color=yellow>Price: {2}</color></size>", equipmentTip, AttackSpeed, SellPrice);
        } else {
            //Adds the attackspeed to the tooltip
            return string.Format("{0} \n <size=20>AttackSpeed: {1}</size>", equipmentTip, AttackSpeed);
        }
    }
}