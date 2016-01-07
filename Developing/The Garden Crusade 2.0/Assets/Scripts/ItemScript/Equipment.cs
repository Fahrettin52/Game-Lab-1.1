using UnityEngine;
using System.Collections;

public class Equipment : Item {

	public int Intellect { get; set; }
	public int Agility { get; set; }
	public int Stamina { get; set; }
	public int Strength { get; set; }

	public Equipment () {
	}

	public Equipment (string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int intellect, int agility, int stamina, int strength) 
		: base(itemName,description,itemType,quality,spriteNeutral,spriteHighlighted,maxSize) {
		this.Intellect = intellect;
		this.Agility = agility;
		this.Stamina = stamina;
		this.Strength = strength;
	}	
	
	public override void Use (Slot slot, ItemScript item)
    {
        CharacterPanel.Instance.EquipItem(slot, item);
	}

    public override string GetToolTip (Inventory inv) {
        string stats = string.Empty;

        if (Strength > 0) //Adds Strength to the tooltip if it is larger than 0
        {
            stats += "\n+" + Strength.ToString() + " Strength";
        }
        if (Intellect > 0) //Adds Intellect to the tooltip if it is larger than 0
        {
            stats += "\n+" + Intellect.ToString() + " Intellect";
        }
        if (Agility > 0)//Adds Agility to the tooltip if it is larger than 0
        {
            stats += "\n+" + Agility.ToString() + " Agility";
        }
        if (Stamina > 0)//Adds Stamina to the tooltip if it is larger than 0
        {
            stats += "\n+" + Stamina.ToString() + " Stamina";
        }

        string itemTip = base.GetToolTip (inv);

        if (inv is VendorInventory && !(this is Weapon)) {
            return string.Format("{0}" + "<size=20>{1}</size>\n<color=yellow>Buy Price: {2} Crumbs</color>", itemTip, stats, BuyPrice);
        } 
        else if (VendorInventory.Instance.IsOpen && !(this is Weapon)) {
            return string.Format("{0}" + "<size=20>{1}</size>\n<color=yellow>Sell Price: {2} Crumbs</color>", itemTip, stats, SellPrice);
        } 
        else {
            return string.Format("{0}" + "<size=20>{1}</size>", itemTip, stats);
        }
    }
}
