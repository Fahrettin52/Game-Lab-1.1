using UnityEngine; 
using System.Collections;

public class Consumeable : Item {

	public int Health { get; set; }
	public int Mana { get; set; }

	public Consumeable () {
	}

	public Consumeable (string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize, int health, int mana) 
		: base(itemName,description,itemType,quality,spriteNeutral,spriteHighlighted,maxSize) {
		this.Health = health;
		this.Mana = mana;
	} 

	public override void Use (Slot slot, ItemScript item) {

        if (ItemName == "Energy Potion" && Stamina.Instance.currentStamina < Stamina.Instance.maxStamina){
            Debug.Log(ItemName);
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
//            PlayerScript.Instance.currentHealth += PlayerScript.Instance.healtPot;
            PlayerScript.Instance.GetHealth();
            PlayerScript.Instance.HandleHealth();
                //if (PlayerScript.Instance.currentHealth >= PlayerScript.Instance.maxHealth){
                //    PlayerScript.Instance.currentHealth = PlayerScript.Instance.maxHealth;
                //}
            slot.RemoveItem();
        }
    }

    public override string GetToolTip () {

        string stats = string.Empty;

        if (Health > 0) {
            stats += "\n Restores " + Health.ToString() + " Health";
        }
        if (Mana > 0)
        {
            stats += "\n Restores " + Mana.ToString() + " Energy";
        }
        string itemTip = base.GetToolTip();

        return string.Format("{0}" + "<size=14>{1}</size>", itemTip, stats);
    }
}