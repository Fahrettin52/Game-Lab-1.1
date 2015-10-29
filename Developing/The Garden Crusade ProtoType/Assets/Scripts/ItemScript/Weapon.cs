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

	public override void Use (Slot slot){
	}

	public override string GetToolTip () {

        string equipmentTip = base.GetToolTip();

        return string.Format("{0} \n <size=14> AttackSpeed: {1}</size>", equipmentTip, AttackSpeed);
    }
}