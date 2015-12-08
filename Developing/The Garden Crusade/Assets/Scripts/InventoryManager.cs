using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Xml.Serialization;
using System.IO;

public class InventoryManager : MonoBehaviour {

    public GameObject[] inventories;
	private static InventoryManager instance;
	public GameObject slotPrefab;
	public GameObject iconPrefab;
	private GameObject hoverObject;
	public GameObject dropItem;
	public GameObject tooltipObject;
	public Text sizeTextObject;
	public Text visualTextObject;
	public Canvas canvas; 	
	private Slot from;	
	private Slot to;	
	private Slot movingSlot;
	private GameObject clicked;
	public Text stackText;
	public GameObject selectStackSize;
	private int splitAmount;
	private int maxStackCount;
	public EventSystem eventSystem;
	public GameObject itemObject;
	private ItemContainer itemContainer = new ItemContainer();

	public ItemContainer ItemContainer {
		get { return itemContainer; }
		set { itemContainer = value; }
	}

	public static InventoryManager Instance {
		get { 
			if (instance == null){
				instance = FindObjectOfType<InventoryManager>();
			}
			return instance;
		}
	}
	
	public Slot From {
		get { return from; }	 	
		set { from = value; }
	}		

	public Slot To {
		get { return to; }
		set { to = value; }
	}

	public GameObject Clicked {
		get { return clicked; }
		set { clicked = value; }
	}

	public int SplitAmount {
		get { return splitAmount; }
		set { splitAmount = value; }
	}
	
	public int MaxStackCount {
		get { return maxStackCount; }
		set { maxStackCount = value; }
	}

	public Slot MovingSlot {
		get { return movingSlot; }
		set { movingSlot = value; }
	}

	public GameObject HoverObject {
		get { return hoverObject; }
		set { hoverObject = value; }
	}

	public Text SizeTextObject {
		get { return sizeTextObject; }
		set { sizeTextObject = value; }
	}

	public void Start () {
		Type[] itemTypes = { typeof(Equipment), typeof(Weapon), typeof(Consumeable), typeof(Material) };
		XmlSerializer serializer = new XmlSerializer (typeof(ItemContainer), itemTypes);
		TextReader textReader = new StreamReader (Application.streamingAssetsPath + "/Items.xml");
		itemContainer = (ItemContainer)serializer.Deserialize (textReader);
		textReader.Close ();
	}

	public void SetStackInfo (int maxStackCount) {
		selectStackSize.SetActive (true);
		tooltipObject.SetActive (false);
		splitAmount = 0;
		this.maxStackCount = maxStackCount;
		stackText.text = splitAmount.ToString ();
	}

    public void Save()
    {
        GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");

        foreach (GameObject inventory in inventories)
        {
            inventory.GetComponent<Inventory>().SaveInventory();
        }
    }

    public void Load () {
		GameObject[] inventories = GameObject.FindGameObjectsWithTag("Inventory");
		
		foreach (GameObject inventory in inventories) {
			inventory.GetComponent<Inventory>().LoadInventory();
		}
	}
}


















