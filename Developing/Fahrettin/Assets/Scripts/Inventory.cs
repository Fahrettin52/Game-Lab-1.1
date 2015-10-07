using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Inventory : MonoBehaviour {

	public RectTransform inventoryRectTransform; 	// Reference to the inventory recttransform
	public Canvas canvas; 							// Reference to the inventorys canvas
	public EventSystem eventSystem; 				// Reference to the eventsystem
	private List<GameObject> allSlots; 
	public int slots;
	public int rows;
	private int splitAmount;
	private int maxStackCount;
	private float inventoryWidth, inventoryHeight;
	public float slotPaddingLeft, slotPaddingTop;
	public float slotSize;
	public float fadeTime;
	private float hoverYOffset;
	public Text stackText;
	public Text sizeTextObject;
	public Text visualTextObject;
	public GameObject tooltipObject;
	public GameObject slotPrefab;
	public GameObject iconPrefab;
	public GameObject selectStackSize;
	private static GameObject selectStackSizeStatic;
	public GameObject mana;	 
	public GameObject health;
	private bool fadingIn;
	private bool fadingOut;
	public static GameObject tooltip;
	private static GameObject clicked;
	private static GameObject hoverObject; 			// Reference to the object that hovers next to the mouse
	private static Slot from; 						// The slot we are moving an item from
	private static Slot to; 						// The slot we are moving an item to
	private static Slot movingSlot;
	private static Text sizeText;
	private static Text visualText;

	// singleton inventory script
	private static Inventory instance;
	public static Inventory Instance {
		get {if (instance == null) {
				instance = GameObject.FindObjectOfType<Inventory>();
			}
			return Inventory.instance;
		}
	}
	// Reference to the canvasgroup in the inventorybackground
	private static CanvasGroup itemGroup; 			
	public static CanvasGroup ItemGroup {
		get { return Inventory.itemGroup; }
	}
	// Reference to the int of the emptyslot
	private static int emptySlots; 					
	public static int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	void Start () {	
		visualText = visualTextObject;
		sizeText = sizeTextObject;
		tooltip = tooltipObject;
		selectStackSizeStatic = selectStackSize;
		itemGroup = transform.GetComponent<CanvasGroup>(); // Set the transform off the itemcanvasgroup to its own transform
		CreateLayout (); // Creates the inventory layout
		movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot> ();
	} 

	void Update () {

		if (Input.GetMouseButtonUp (0)) { // Checks if the user lifted the first mousebutton
			// Removes the selected item from the inventory
			if (!eventSystem.IsPointerOverGameObject (-1) && from != null) { // If we click outside the inventory and we have picked up an item
				from.GetComponent<Image> ().color = Color.white; // reset the slots color
				from.ClearSlot (); // Removes the item from the slot
				Destroy (GameObject.Find ("Hover")); // Removes the hover icon
				// Resets the object
				to = null;
				from = null;
				emptySlots ++;
			} else if (!eventSystem.IsPointerOverGameObject (-1) && !movingSlot.IsEmpty){
				movingSlot.ClearSlot();
				Destroy (GameObject.Find ("Hover"));
			}
		}
		if (hoverObject != null) { // Checks if the hoverobject exists
			Vector2 position; // The hoverobject position
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position); // Translates mouse screen position in a local position and stores it in the position
			position.Set(position.x, position.y - hoverYOffset); // Adds the offset to the position 
			hoverObject.transform.position = canvas.transform.TransformPoint(position); // Sets the hoverobject position
		}

		if (Input.GetKeyDown (KeyCode.I)) {
			if (itemGroup.alpha > 0){
				StartCoroutine("FadeOut");
				PutItemBack();
			}
			else {
				StartCoroutine("FadeIn");
			}
		}
		if (Input.GetMouseButton(2)) {
			if (eventSystem.IsPointerOverGameObject(-1)) {
				MoveInventory();
			}
		}
	}

	public void ShowToolTip (GameObject slot) {
		Slot tmpSlot =  slot.GetComponent<Slot>(); // temp variable = slot

		if (!tmpSlot.IsEmpty && hoverObject == null && !selectStackSizeStatic.activeSelf) { // if the slot is empty and the hoverobject is null and the selectstacksize is not active
			visualText.text = tmpSlot.CurrentItem.GetToolTip();
			sizeText.text = visualText.text;
			tooltip.SetActive (true); // setactive true gameobject
			float xPos = slot.transform.position.x - slotPaddingLeft; // to set the posiiton to the right
			float yPos = slot.transform.position.y - slot.GetComponent<RectTransform>().sizeDelta.y;// this is to set it to under the slot with the - width of the slot and toppadding
			tooltip.transform.position = new Vector2(xPos, yPos); // here we drop the 2 positions in the gameobject transform
		}
	}

	public void HideToolTip () {
		tooltip.SetActive (false);
	}

	public void SaveInventory () {
		string content = string.Empty;
		for (int i = 0; i < allSlots.Count; i++) {
			Slot tmp = allSlots[i].GetComponent<Slot>();
			if (!tmp.IsEmpty) {
				content += i + "-" + tmp.CurrentItem.type.ToString() + "-" + tmp.Items.Count.ToString() + ";";
			}
		}
		PlayerPrefs.SetString ("content", content);
		PlayerPrefs.SetInt ("slots", slots);
		PlayerPrefs.SetInt ("rows", rows);
		PlayerPrefs.SetFloat ("slotPaddingLeft", slotPaddingLeft);
		PlayerPrefs.SetFloat ("slotPaddingTop", slotPaddingTop);
		PlayerPrefs.SetFloat ("slotSize", slotSize);
		PlayerPrefs.SetFloat ("xPos", inventoryRectTransform.position.x);
		PlayerPrefs.SetFloat ("yPos", inventoryRectTransform.position.y);
		PlayerPrefs.Save ();
	} 

	public void LoadInventory () {
		string content = PlayerPrefs.GetString ("content");
		slots = PlayerPrefs.GetInt ("slots");
		rows = PlayerPrefs.GetInt ("rows");
		slotPaddingLeft = PlayerPrefs.GetFloat ("slotPaddingLeft");
		slotPaddingTop = PlayerPrefs.GetFloat ("slotPaddingTop");
		slotSize = PlayerPrefs.GetFloat ("slotSize");
		inventoryRectTransform.position = new Vector3 (PlayerPrefs.GetFloat ("xPos"), PlayerPrefs.GetFloat ("yPos"), inventoryRectTransform.position.z);
		CreateLayout ();
		string[] splitContent = content.Split (';'); 

		for (int x = 0; x < splitContent.Length-1; x++) { // 0-MANA-3
			string[] splitValues = splitContent[x].Split('-'); 
			int index = Int32.Parse(splitValues[0]); // converts string to int 0,1 etc
			ItemType type = (ItemType)Enum.Parse(typeof(ItemType), splitValues[1]); // converts the type to MANA or HEALTH
			int amount = Int32.Parse(splitValues[2]); // amount of pots 

			for (int i = 0; i < amount; i++){
				switch (type) {
					case ItemType.MANA:
						allSlots[index].GetComponent<Slot>().AddItem(mana.GetComponent<Item>());
						break;
					case ItemType.HEALTH:
					allSlots[index].GetComponent<Slot>().AddItem(health.GetComponent<Item>());
						break;
				}
			}
		}
	}

	private void CreateLayout(){ // creates inventory layout

		if (allSlots != null) {
			foreach (GameObject go in allSlots) {
				Destroy(go);
			}
		}
		allSlots = new List<GameObject> (); // instantiates the allslot list

		hoverYOffset = slotSize * 0.1f; // icon met 1 % omlaag gaat

		emptySlots = slots; // stores the number of empty slots

		inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft; // calculates the width of the inventory

		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop; // calculates the height of the inventory

		inventoryRectTransform = GetComponent<RectTransform> (); // creates a reference to the inventorys recttransform

		inventoryRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth); // sets the width of the inventory
		inventoryRectTransform.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight); // sets the height of the inventory

		int columns = slots / rows; // calculates the amount of columns
			 
		for (int y = 0; y < rows; y++) { // runs through the rows
			for (int x = 0; x < columns; x++) { // runs through the colums
				GameObject newSlot = Instantiate(slotPrefab); // instantiate a slot and creates a reference to it

				RectTransform slotRect = newSlot.GetComponent<RectTransform>(); // makes a reference to the rect transform

				newSlot.name = "Slot "+"x"+x+"y"+y; // set the slot name

				newSlot.transform.SetParent(this.transform); // set the canvas as parent of the slot so that it is visible on the screen

				slotRect.localPosition = new Vector3(slotPaddingLeft * (x+1) + (slotSize * x), -slotPaddingTop * (y+1) - (slotSize * y)); // set the slot position

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * canvas.scaleFactor); // set the size of the slot
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * canvas.scaleFactor); // set the size of the slot

				allSlots.Add(newSlot); // add the new slot to the slot lost
			}
		}
	}

	public bool AddItem(Item item){ // adds an item to the inventory
		if (item.maxSize == 1) { // if the item isnt stackable
			PlaceEmpty (item); // place an item at the empty slot
			return true;
		} 
		else { // if the item is stackable
			foreach (GameObject slot in allSlots) { // runs through all slots in the inventory
				Slot tmp = slot.GetComponent<Slot> (); // creates reference to the slot

				if (!tmp.IsEmpty){ // if the item isnt empty
					if (tmp.CurrentItem.type == item.type && tmp.IsAvailabele){ //checks of the om the slot is the same type as the item we want to pick up

						if (!movingSlot.IsEmpty && clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>()) {
							continue;  // loops again
						}
						else {
							tmp.AddItem(item); // adds the item to the inventory
							return true;
						}
					}
				}
			}
			if (emptySlots > 0) { // places the item on the empty slot
				PlaceEmpty(item);
			}
		}
		return false;
	}

	private void MoveInventory () {
		Vector2 mousePos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, new Vector3(Input.mousePosition.x - (inventoryRectTransform.sizeDelta.x / 2 * canvas.scaleFactor), Input.mousePosition.y + (inventoryRectTransform.sizeDelta.y/2 * canvas.scaleFactor)), canvas.worldCamera, out mousePos);
		transform.position = canvas.transform.TransformPoint(mousePos);
	}

	// places an item on the empty slot
	private bool PlaceEmpty (Item item) {
		if (emptySlots > 0) { // if we at least have 1 empty slot
			foreach (GameObject slot in allSlots) { // run through all slots
				Slot tmp = slot.GetComponent<Slot> (); // creates a reference to the slot
		
				if (tmp.IsEmpty){ // if the slot is empty
					tmp.AddItem(item); // add the item
					emptySlots --; // reduce the number of empty slots
					return true;
				}
			}
		}
		return false;
	}
	// moves a item to another slot in the inventory
	public void MoveItem(GameObject clicked){ 
		Inventory.clicked = clicked;
		if (!movingSlot.IsEmpty) {
			Slot tmp = clicked.GetComponent<Slot>();
			if (tmp.IsEmpty) {
				tmp.AddItems(movingSlot.Items);
				movingSlot.Items.Clear();
				Destroy(GameObject.Find("Hover"));
			}
			else if (!tmp.IsEmpty && movingSlot.CurrentItem.type == tmp.CurrentItem.type && tmp.IsAvailabele) {
				MergeStacks(movingSlot, tmp);	 
			}
		}

		else if (from == null && itemGroup.alpha == 1 && !Input.GetKey(KeyCode.LeftShift)) { // if we havent picked an item only when the inventory is 100% visible
			if (!clicked.GetComponent <Slot>().IsEmpty && !GameObject.Find("Hover")){ // if the slot we clicked isnt empty
				from = clicked.GetComponent<Slot>(); // the slot we are moving from
				from.GetComponent<Image>().color = Color.gray; // sets the from slot color to gray, to visually indicate that its the slot we are moving
				CreateHoverIcon();
			}
		}
		else if (to == null && !Input.GetKey(KeyCode.LeftShift) ) { // select the slot we are moving to
			to = clicked.GetComponent<Slot>(); // set the object
			Destroy(GameObject.Find("Hover")); // destroy the hover object
		}

		if (to != null && from != null) { // if both to and from are null than we are done moving 
			if (!to.IsEmpty && from.CurrentItem.type == to.CurrentItem.type && to.IsAvailabele) {
				MergeStacks(from, to);
			}
			else {
				Stack<Item> tmpTo = new Stack<Item>(to.Items); // stores the itemf from the to slot, so we can do a swap
				to.AddItems(from.Items); // clear the from slot

				if (tmpTo.Count == 0) { // if to slot is 0 then we dont need to move anything to the from slot
					from.ClearSlot(); // clear the from slot
				}
				else {
				from.AddItems(tmpTo); // if the to slot contains item then we need to move the to the from slot
				}
			}
			// reset all values
			from.GetComponent<Image>().color = Color.white;
			to = null;
			from = null; 
			Destroy(GameObject.Find("Hover"));
		}
	}

	private void CreateHoverIcon () {
		hoverObject = (GameObject)Instantiate(iconPrefab); // instantiates the hover object
		hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite; // sets the sprite on the hover object so that it reflects the object we are moving
		hoverObject.name = "Hover"; // sets the name of the hover object
		
		RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>(); // creates a reference to the transform
		RectTransform clickedTransform = clicked.GetComponent<RectTransform>(); // creates a reference to the transform
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x); // set the size of the hoverobject so that it has the same size as the clicked object
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y); // set the size of the hoverobject so that it has the same size as the clicked object
		
		hoverObject.transform.SetParent(GameObject.Find("InventoryBackground").transform, true); // sets the hoverobjects parent as the InventoryBackground so that its visible in the game
		hoverObject.transform.localScale = clicked.gameObject.transform.localScale; // sets the local scale to make sure that it has the correct size
		hoverObject.transform.GetChild (0).GetComponent<Text> ().text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString () : string.Empty;
	}

	private void PutItemBack () { // put item back to the inventory when inventory closing while having the hover
		if (from != null) {
			Destroy (GameObject.Find ("Hover")); // destroys the hovericon
			from.GetComponent<Image> ().color = Color.white;
			from = null;
		} 
		else if (!movingSlot.IsEmpty) {
			Destroy (GameObject.Find ("Hover"));
			foreach (Item item in movingSlot.Items) {
				clicked.GetComponent<Slot>().AddItem(item);
			}
			movingSlot.ClearSlot();
		}
		selectStackSize.SetActive (false);
	}

	public void SetStackInfo (int maxStackCount) {
		selectStackSize.SetActive (true);
		splitAmount = 0;
		this.maxStackCount = maxStackCount;
		stackText.text = splitAmount.ToString ();
	}

	public void SplitStack () {
		selectStackSize.SetActive (false);
		tooltip.SetActive (false);
		if (splitAmount == maxStackCount) {
			MoveItem(clicked);
		}
		else if (splitAmount > 0) {
			movingSlot.Items = clicked.GetComponent<Slot>().RemoveItems(splitAmount);
			CreateHoverIcon();
		}
	}

	public void ChangeStackText (int i) {
		splitAmount += i;
		if (splitAmount < 0) {	
			splitAmount = 0;
		}
		if (splitAmount > maxStackCount) {
			splitAmount = maxStackCount;
		}
		stackText.text = splitAmount.ToString ();
	}

	public void MergeStacks (Slot source, Slot destination) {
		int max = destination.CurrentItem.maxSize - destination.Items.Count;
		int count = source.Items.Count < max ? source.Items.Count : max;
		for (int i = 0; i < count; i++) {
			destination.AddItem(source.RemoveItem());
			hoverObject.transform.GetChild (0).GetComponent<Text> ().text = movingSlot.Items.Count.ToString();	
		}
		if (source.Items.Count == 0) {
			source.ClearSlot();
			Destroy(GameObject.Find("Hover")); 	
		}
	}

	private IEnumerator FadeOut () {
		if (!fadingOut) {
			fadingOut = true;
			fadingIn = false ;
			StopCoroutine ("FadeIn");

			float startAlpha = itemGroup.alpha; // current alpha
			float rate = 1f / fadeTime;
			float progress = 0.0f;

			while (progress < 1.0) {
				itemGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
				progress += rate * Time.deltaTime;
				yield return null;
			}

			itemGroup.alpha = 0;
			fadingOut = false;
		}
	}

	private IEnumerator FadeIn () {
		if (!fadingIn) {
			fadingOut = false;
			fadingIn = true;
				StopCoroutine ("FadeOut");
			
			float startAlpha = itemGroup.alpha; // current alpha
			float rate = 1f / fadeTime;
			float progress = 0.0f;
			
			while (progress < 1.0) {
				itemGroup.alpha = Mathf.Lerp(startAlpha, 1, progress);
				progress += rate * Time.deltaTime;
				yield return null;
			}
			
			itemGroup.alpha = 1;
			fadingIn = false;
		}
	}
}
















