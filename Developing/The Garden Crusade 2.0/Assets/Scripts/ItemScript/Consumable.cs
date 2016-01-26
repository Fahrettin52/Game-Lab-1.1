using UnityEngine; 
using System.Collections;

public class Consumeable : Item {

	public int Health { get; set; }
	public int Mana { get; set; }
    public int Rage { get; set; }

    public Consumeable () {
	}

	public Consumeable (string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int health, int mana, int rage) 
		: base(itemName,description,itemType,quality,spriteNeutral,spriteHighlighted,maxSize) {
		this.Health = health;
		this.Mana = mana;
        this.Rage = Rage;
	} 

	public override void Use (Slot slot, ItemScript item) {

        if (ItemName == "Energy Potion" && Stamina.Instance.currentStamina < Stamina.Instance.maxStamina){
            Stamina.Instance.currentStamina += Stamina.Instance.energyPot;
            Stamina.Instance.bar.fillAmount += Stamina.Instance.energyPot / 100f;
            Stamina.Instance.ManaColor();
            Stamina.Instance.ManaText();
                if(Stamina.Instance.currentStamina >= Stamina.Instance.maxStamina) {
                    Stamina.Instance.currentStamina = Stamina.Instance.maxStamina;
                }
            slot.RemoveItem();
        }
        if (ItemName == "Health Potion" && PlayerScript.Instance.currentHealth < PlayerScript.Instance.maxHealth) {
            Debug.Log(ItemName);
            PlayerScript.Instance.GetHealth();
            PlayerScript.Instance.HandleHealth();
            slot.RemoveItem();
        }
        if (ItemName == "Rage Potion" && Stamina.Instance.currentRage < Stamina.Instance.maxRage) {
            Stamina.Instance.RagePotion();
            slot.RemoveItem();
        }
    }

    public override string GetToolTip (Inventory inv) {

        string stats = string.Empty;

        if (Health > 0) {
            stats += "\nRestores " + Health.ToString() + " health";
        }
        if (Mana > 0)
        {
            stats += "\nRestores " + Mana.ToString() + " energy";
        }
        if (Rage > 0)
        {
            stats += "\nRestores " + Rage.ToString() + " rage";
        }
        string itemTip = base.GetToolTip(inv);

        if (inv is VendorInventory) {
            return string.Format("{0}" + "<size=20>{1}\n<color=yellow>Buy Price: {2} Crumbs</color></size>", itemTip, stats, BuyPrice);
        } 
        else if (VendorInventory.Instance.IsOpen) {
            return string.Format("{0}" + "<size=20>{1}\n<color=yellow>Sell Price: {2} Crumbs</color></size>", itemTip, stats, SellPrice);
        } 
        else {
            return string.Format("{0}" + "<size=20>{1}</size>", itemTip, stats);
        }

    }
}