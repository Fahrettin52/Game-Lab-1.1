 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemContainer {

	private List<Item> weapons = new List<Item>();
	private List<Item> equipment = new List<Item>();
	private List<Item> consumeable = new List<Item>();

    public List<Item> Weapons {
		get { return weapons; }
		set { weapons = value;}
	}
	
	public List<Item> Equipment {
		get { return equipment; }
		set { equipment = value; }
	}

	public List<Item> Consumable {
		get { return consumeable; }
		set { consumeable = value;}
	}

    public ItemContainer () {
	}	 
}