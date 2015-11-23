using UnityEngine;
using System.Collections;

public enum ItemType {CONSUMEABLE, MAINHAND, OFFHAND, TWOHAND, SHIELD, HEAD, NECK, CHEST, RING, LEGS, BRACES, BOOTS, TRINKET, SWORD, STAMINA, SHOULDER, PANTS, BELT, GENERIC, GENERICWEAPON}
public enum Quality	{COMMON, UNCOMMON, RARE, EPIC, LEGENDARY};

public class ItemScript : MonoBehaviour {
	
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public GameObject player;
	private Item item;
    //public ItemType type;

    public Item Item {
		get { return item; }
		set { 
			item = value; 
			spriteHighlighted = Resources.Load<Sprite>(value.SpriteHighlighted);
			spriteNeutral = Resources.Load<Sprite>(value.SpriteNeutral);
		}
	}

	public void Awake () {
        player = GameObject.FindWithTag ("Player");
	}

	public void Use (Slot slot)
    {
        item.Use(slot, this);
}

	public string GetToolTip () {
        return item.GetToolTip();
	}
}





























