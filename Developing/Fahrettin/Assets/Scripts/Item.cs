using UnityEngine;
using System.Collections;

public enum ItemType {MANA, HEALTH, SWORD, STAMINA};
public enum Quality	{COMMON, UNCOMMON, RARE, EPIC, LEGENDARY};

public class Item : MonoBehaviour {

	public ItemType type;
	public Quality quality;
	public Sprite spriteNeutral;
	public Sprite spriteHighlighted;
	public int maxSize;
	public float strength, defence, attackspeed, energy, health;
	public string itemName;
	public string description;

	// uses the item
	public void Use () {
		switch (type) {	// checks what kind of item this is
			case ItemType.MANA:
			Debug.Log("I used a mana pot");
				break;
			case ItemType.HEALTH:
			Debug.Log("I used a health pot");
				break;
		}
	}

	public string GetToolTip () {
		string stats = string.Empty;
		string color = string.Empty;
		string newLine = string.Empty;

		if (description != string.Empty) { // if our description contains something
			newLine = "\n";
		}

		switch (quality) {
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

		if (strength > 0) {
			stats += "\n+" + strength.ToString() + " Strength";
		}
		if (defence > 0) {
			stats += "\n+" + defence.ToString() + " Defence";
		}
		if (attackspeed > 0) {
			stats += "\n+" + attackspeed.ToString() + " Attackspeed";
		}
		if (energy > 0) {
			stats += "\n+" + energy.ToString() + " Energy";
		}
		if (health > 0) {
			stats += "\n+" + health.ToString() + " Health";
		}
		return string.Format("<color=" + color + "><size=16>{0}</size></color><size=14><i><color=lime>" + newLine + "{1}</color></i>{2}</size>", itemName, description, stats);
	}
}
































