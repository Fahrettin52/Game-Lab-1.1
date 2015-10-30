using UnityEngine;
using System.Collections;

public abstract class Item {
	public ItemType	ItemType { get; set; }
	public Quality 	Quality { get; set; }
	public string 	SpriteNeutral { get; set; }
	public string 	SpriteHighlighted { get; set; }
	public int 		MaxSize { get; set; }
	public string 	ItemName { get; set; }
	public string 	Description { get; set; }

	public Item () {
	}

	public Item (string itemName, string description, ItemType itemType, Quality quality, string spriteNeutral, string spriteHighlighted, int maxSize) {
		this.ItemName 			= itemName;
		this.Description 		= description;
		this.ItemType			= itemType;
		this.Quality 			= quality;
		this.SpriteNeutral 		= spriteNeutral;
		this.SpriteHighlighted 	= spriteHighlighted;
		this.MaxSize 			= maxSize;
	}

	public abstract void Use (Slot slot);

    public virtual string GetToolTip () {
        //string stats = string.Empty;
        string color = string.Empty;
        string newLine = string.Empty;

        if (Description != string.Empty) {
            newLine = "\n";
        }

        switch (Quality) {
            case Quality.COMMON:
                color = "white";
                break;
            case Quality.UNCOMMON:
                color = "lime";
                break;
            case Quality.RARE:
                color = "navy";
                break;
            case Quality.EPIC:
                color = "magenta";
                break;
            case Quality.LEGENDARY:
                color = "red";
                break;
        }

        return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i></size>", ItemName,Description);
	}

}