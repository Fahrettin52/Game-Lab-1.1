using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Inventory : MonoBehaviour {

    private static Inventory instance;

    public static Inventory Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Inventory>();
            }
            return instance;
        }
    }

    public 	int rows;
	public 	int slots;
    public int emptySlots;  
	protected float hoverYOffset;
	protected float inventoryWidth, inventoryHeight;
	public 	float slotPaddingLeft, slotPaddingTop;
	public 	float slotSize;
	protected RectTransform inventoryRect; 
	public CanvasGroup itemGroup;
    public CanvasGroup buttonGroup;
	private bool fadingIn;
	private bool fadingOut;
    public GameObject soundToOpen;
    public float lifeTimeSound;

    public bool FadingOut {
        get { return fadingOut; }
    }

	public 	float fadeTime;
	protected List<GameObject> allSlots; 
	private bool isOpen;
	public static bool mouseInside = false;
    public bool instantClose = false;

    public bool MouseInside {
        get { return mouseInside; }
        set { mouseInside = value; }
    }

    public bool IsOpen {
		get { return isOpen; }
		set { isOpen = value; }
	}

	public int EmptySlots {
		get { return emptySlots; }
		set { emptySlots = value; }
	}

	protected static GameObject playerRef;

	protected virtual void Start () {
        isOpen = false;
        playerRef = GameObject.Find("Player");
        itemGroup = GetComponent<CanvasGroup>();
        CreateLayout();
        InventoryManager.Instance.MovingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
    } 

	void Update () {

		if (Input.GetMouseButtonUp (0)) { 
			if (!mouseInside && InventoryManager.Instance.From != null) { 
				InventoryManager.Instance.From.GetComponent<Image> ().color = Color.white; 
				foreach (ItemScript item in InventoryManager.Instance.From.Items) { 
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
					Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
					v *= 2;
					GameObject tmpDrp = (GameObject)GameObject.Instantiate(InventoryManager.Instance.dropItem, playerRef.transform.position - v, Quaternion.identity);
                    tmpDrp.AddComponent<ItemScript>();
                    tmpDrp.GetComponent<ItemScript>().Item = item.Item;
				}

				InventoryManager.Instance.From.ClearSlot ();
                
                if (InventoryManager.Instance.From.transform.parent == CharacterPanel.Instance.transform)
                {
                    CharacterPanel.Instance.CalculateStats();
                }

				Destroy (GameObject.Find ("Hover")); 
				InventoryManager.Instance.To = null;
				InventoryManager.Instance.From = null;

			} else if (!InventoryManager.Instance.eventSystem.IsPointerOverGameObject (-1) && !InventoryManager.Instance.MovingSlot.IsEmpty){
				foreach (ItemScript item in InventoryManager.Instance.MovingSlot.Items) { 
					float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
					Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
					v *= 2;
					GameObject tmpDrp = (GameObject)GameObject.Instantiate(InventoryManager.Instance.dropItem, playerRef.transform.position - v, Quaternion.identity);
                    tmpDrp.AddComponent<ItemScript>();
                    tmpDrp.GetComponent<ItemScript>().Item = item.Item;
                }
				InventoryManager.Instance.MovingSlot.ClearSlot();
				Destroy (GameObject.Find ("Hover"));
			}
		}
		if (InventoryManager.Instance.HoverObject != null) { 
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position); // Translates mouse screen position in a local position and stores it in the position
			position.Set(position.x, position.y - hoverYOffset); 
			InventoryManager.Instance.HoverObject.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(position); // Sets the hoverobject position
		}
	}

	public void OnDrag () {
        if (isOpen) {
            MoveInventory();
        }
	}

    public void PointerExit(){
        mouseInside = false;
    }

    public void PointerEnter () {
		if (itemGroup.alpha > 0) {
			mouseInside = true;
		}
    }

    public void ButtonEnter(){
        if (buttonGroup.alpha == 1){
            mouseInside = true;
        }
    }

    public void ButtonExit(){
        mouseInside = false;
    }

    public virtual void Open () {
        OpenInventorySound();
        if (itemGroup.alpha > 0){
			StartCoroutine("FadeOut");
			PutItemBack();
			HideToolTip(); 
			isOpen = false;
		}
		else {
			StartCoroutine("FadeIn");
			isOpen = true;
		}
	}

	public virtual void ShowToolTip (GameObject slot) {
		Slot tmpSlot =  slot.GetComponent<Slot>(); 
		if (!tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf && slot.GetComponentInParent<Inventory>().isOpen ) {   
			InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetToolTip(this);
			InventoryManager.Instance.SizeTextObject.text = InventoryManager.Instance.visualTextObject.text;
			InventoryManager.Instance.tooltipObject.SetActive (true);
            float xPos = slot.transform.position.x + 1  ;
            float yPos = slot.transform.position.y  - slot.GetComponent<RectTransform>().sizeDelta.y -8;
			InventoryManager.Instance.tooltipObject.transform.position = new Vector2(xPos, yPos); 
		}
	}

	public void HideToolTip () {
		InventoryManager.Instance.tooltipObject.SetActive (false);

	}

    public virtual void SaveInventory()
    {
        print("inventory save");
        string content = string.Empty; //Creates a string for containing infor about the items inside the inventory

        for (int i = 0; i < allSlots.Count; i++) //Runs through all slots in the inventory
        {
            Slot tmp = allSlots[i].GetComponent<Slot>(); //Careates a reference to the slot at the current index

            if (!tmp.IsEmpty) //We only want to save the info if the slot contains an item
            {
                //Creates a string with this format: SlotIndex-ItemType-AmountOfItems; this string can be read so that we can rebuild the inventory
                content += i + "-" + tmp.CurrentItem.Item.ItemName.ToString() + "-" + tmp.Items.Count.ToString() + ";";
            }
        }

        //Stores all the info in the PlayerPrefs
        PlayerPrefs.SetString(gameObject.name + "content", content);
        PlayerPrefs.SetInt(gameObject.name + "slots", slots);
        PlayerPrefs.SetInt(gameObject.name + "rows", rows);
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingLeft", slotPaddingLeft);
        PlayerPrefs.SetFloat(gameObject.name + "slotPaddingTop", slotPaddingTop);
        PlayerPrefs.SetFloat(gameObject.name + "slotSize", slotSize);
        PlayerPrefs.SetFloat(gameObject.name + "xPos", inventoryRect.position.x);
        PlayerPrefs.SetFloat(gameObject.name + "yPos", inventoryRect.position.y);
        PlayerPrefs.Save();
        //CharacterPanel.Instance.SaveInventory();
    }

    public virtual void LoadInventory()
    {
        print("inventory load");
        //CharacterPanel.Instance.LoadInventory();
        //Loads all the inventory's data from the playerprefs
        string content = PlayerPrefs.GetString(gameObject.name + "content");

        if (content != string.Empty)
        {
            slots = PlayerPrefs.GetInt(gameObject.name + "slots");
            rows = PlayerPrefs.GetInt(gameObject.name + "rows");
            slotPaddingLeft = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingLeft");
            slotPaddingTop = PlayerPrefs.GetFloat(gameObject.name + "slotPaddingTop");
            slotSize = PlayerPrefs.GetFloat(gameObject.name + "slotSize");

            //Sets the inventorys position
            inventoryRect.position = new Vector3(PlayerPrefs.GetFloat(gameObject.name + "xPos"), PlayerPrefs.GetFloat(gameObject.name + "yPos"), inventoryRect.position.z);

            //Recreates the inventory's layout
            CreateLayout();

            //Splits the loaded content string into segments, so that each index inthe splitContent array contains information about a single slot
            //e.g[0]0-MANA-3
            string[] splitContent = content.Split(';');

            //Runs through every single slot we have infor about -1 is to avoid an empty string error
            for (int x = 0; x < splitContent.Length - 1; x++)
            {
                //Splits the slot's information into single values, so that each index in the splitValues array contains info about a value
                //E.g[0]InventorIndex [1]ITEMTYPE [2]Amount of items
                string[] splitValues = splitContent[x].Split('-');

                int index = Int32.Parse(splitValues[0]); //InventorIndex 

                string itemName = splitValues[1]; //ITEMTYPE

                int amount = Int32.Parse(splitValues[2]); //Amount of items

                Item tmp = null;

                for (int i = 0; i < amount; i++) //Adds the correct amount of items to the inventory
                {
                    GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);

                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContainer.Consumable.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContainer.Equipment.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null)
                    {
                        tmp = InventoryManager.Instance.ItemContainer.Weapons.Find(item => item.ItemName == itemName);
                    }
                    if (tmp == null) {
                        tmp = InventoryManager.Instance.ItemContainer.Materials.Find(item => item.ItemName == itemName);
                    }

                    loadedItem.AddComponent<ItemScript>();
                    loadedItem.GetComponent<ItemScript>().Item = tmp;
                    allSlots[index].GetComponent<Slot>().AddItem(loadedItem.GetComponent<ItemScript>());
                    Destroy(loadedItem);
                }
            }
        }



    }

    public virtual void CreateLayout(){ // creates inventory layout

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

		inventoryRect = GetComponent<RectTransform> (); // creates a reference to the inventorys recttransform

		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, inventoryWidth); // sets the width of the inventory
		inventoryRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, inventoryHeight); // sets the height of the inventory

		int columns = slots / rows; // calculates the amount of columns
			 
		for (int y = 0; y < rows; y++) { // runs through the rows
			for (int x = 0; x < columns; x++) { // runs through the colums
				GameObject newSlot = (GameObject)Instantiate(InventoryManager.Instance.slotPrefab); // instantiate a slot and creates a reference to it
					
				RectTransform slotRect = newSlot.GetComponent<RectTransform>(); // makes a reference to the rect transform

				newSlot.name = "Slot "+"x"+x+"y"+y; // set the slot name

				newSlot.transform.SetParent(this.transform); // set the canvas as parent of the slot so that it is visible on the screen

				slotRect.localPosition = new Vector3(slotPaddingLeft * (x+1) + (slotSize * x), -slotPaddingTop * (y+1) - (slotSize * y)); // set the slot position

				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize * InventoryManager.Instance.canvas.scaleFactor); // set the size of the slot
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize * InventoryManager.Instance.canvas.scaleFactor); // set the size of the slot

				allSlots.Add(newSlot); // add the new slot to the slot lost

                newSlot.GetComponent<Button>().onClick.AddListener(
                    delegate { MoveItem(newSlot); }     
                );
			}
		}
	}

	public bool AddItem(ItemScript item){ // adds an item to the inventory
		if (item.Item.MaxSize == 1) { // if the item isnt stackable
			return PlaceEmpty (item); // place an item at the empty slot
		} 
		else { // if the item is stackable
			foreach (GameObject slot in allSlots) { // runs through all slots in the inventory
				Slot tmp = slot.GetComponent<Slot> (); // creates reference to the slot

				if (!tmp.IsEmpty){ // if the item isnt empty
					if (tmp.CurrentItem.Item.ItemName == item.Item.ItemName && tmp.IsAvailabele){ //checks of the om the slot is the same type as the item we want to pick up

						if (!InventoryManager.Instance.MovingSlot.IsEmpty && InventoryManager.Instance.Clicked.GetComponent<Slot>() == tmp.GetComponent<Slot>()) {
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
				return PlaceEmpty(item);
			}
		}
		return false;
	}

	private void MoveInventory () {
		Vector2 mousePos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(InventoryManager.Instance.canvas.transform as RectTransform, new Vector3(Input.mousePosition.x - (inventoryRect.sizeDelta.x / 2 * InventoryManager.Instance.canvas.scaleFactor), Input.mousePosition.y + (inventoryRect.sizeDelta.y / 2 * InventoryManager.Instance.canvas.scaleFactor)), InventoryManager.Instance.canvas.worldCamera, out mousePos);
		transform.position = InventoryManager.Instance.canvas.transform.TransformPoint(mousePos);
	}

	// places an item on the empty slot
	private bool PlaceEmpty (ItemScript item) {
		if (emptySlots > 0) { // if we at least have 1 empty slot
			foreach (GameObject slot in allSlots) { // run through all slots
				Slot tmp = slot.GetComponent<Slot> (); // creates a reference to the slot
		
				if (tmp.IsEmpty){ // if the slot is empty
					tmp.AddItem(item); // add the item
					return true;
				}
			}
		}
		return false;
	}
	// moves a item to another slot in the inventory
	public virtual void MoveItem(GameObject clicked){
            CanvasGroup cg = clicked.transform.parent.GetComponent<CanvasGroup>();

        if (cg != null && cg.alpha > 0 || clicked.transform.parent.parent.GetComponent<CanvasGroup>().alpha > 0)
        {
			InventoryManager.Instance.Clicked = clicked;
			if (!InventoryManager.Instance.MovingSlot.IsEmpty) {
				Slot tmp = clicked.GetComponent<Slot> ();
				if (tmp.IsEmpty) {
					tmp.AddItems (InventoryManager.Instance.MovingSlot.Items);
					InventoryManager.Instance.MovingSlot.Items.Clear (); 
					Destroy (GameObject.Find ("Hover"));
				} 
				else if (!tmp.IsEmpty && InventoryManager.Instance.MovingSlot.CurrentItem.Item.ItemName == tmp.CurrentItem.Item.ItemName && tmp.IsAvailabele) {
					MergeStacks (InventoryManager.Instance.MovingSlot, tmp);	 
				}
			} else if (InventoryManager.Instance.From == null && clicked.transform.parent.GetComponent<Inventory> ().isOpen && !Input.GetKey (KeyCode.LeftShift)) { // if we havent picked an item only when the inventory is 100% visible
				if (!clicked.GetComponent <Slot> ().IsEmpty && !GameObject.Find ("Hover")) { // if the slot we clicked isnt empty
					InventoryManager.Instance.From = clicked.GetComponent<Slot> (); // the slot we are moving from
					InventoryManager.Instance.From.GetComponent<Image> ().color = Color.gray; // sets the from slot color to gray, to visually indicate that its the slot we are moving
					CreateHoverIcon ();
				}
			} else if (InventoryManager.Instance.To == null && !Input.GetKey (KeyCode.LeftShift)) { // select the slot we are moving to
				InventoryManager.Instance.To = clicked.GetComponent<Slot> (); // set the object
				Destroy (GameObject.Find ("Hover")); // destroy the hover object
			}

			if (InventoryManager.Instance.To != null && InventoryManager.Instance.From != null) { // if both to and from are null than we are done moving 
				if (!InventoryManager.Instance.To.IsEmpty && InventoryManager.Instance.From.CurrentItem.Item.ItemName == InventoryManager.Instance.To.CurrentItem.Item.ItemName && InventoryManager.Instance.To.IsAvailabele) {
					MergeStacks (InventoryManager.Instance.From, InventoryManager.Instance.To);
				}
                else {
                    Slot.SwapItems(InventoryManager.Instance.From, InventoryManager.Instance.To);
				}
				// reset all values
				InventoryManager.Instance.From.GetComponent<Image> ().color = Color.white;
				InventoryManager.Instance.To = null;
				InventoryManager.Instance.From = null; 
				Destroy (GameObject.Find ("Hover"));
			}
		}

        if (CraftingBench.Instance.isOpen) {
            CraftingBench.Instance.UpdatePreview();
        }
	}

	private void CreateHoverIcon () {
		InventoryManager.Instance.HoverObject = (GameObject)Instantiate(InventoryManager.Instance.iconPrefab); // instantiates the hover object
		InventoryManager.Instance.HoverObject.GetComponent<Image>().sprite = InventoryManager.Instance.Clicked.GetComponent<Image>().sprite; // sets the sprite on the hover object so that it reflects the object we are moving
		InventoryManager.Instance.HoverObject.name = "Hover"; // sets the name of the hover object
		
		RectTransform hoverTransform = InventoryManager.Instance.HoverObject.GetComponent<RectTransform>(); // creates a reference to the transform
		RectTransform clickedTransform = InventoryManager.Instance.Clicked.GetComponent<RectTransform>(); // creates a reference to the transform
		
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x); // set the size of the hoverobject so that it has the same size as the clicked object
		hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y); // set the size of the hoverobject so that it has the same size as the clicked object
		
		InventoryManager.Instance.HoverObject.transform.SetParent(GameObject.Find("InventoryBackground").transform, true); // sets the hoverobjects parent as the InventoryBackground so that its visible in the game
		InventoryManager.Instance.HoverObject.transform.localScale = InventoryManager.Instance.Clicked.gameObject.transform.localScale; // sets the local scale to make sure that it has the correct size
		InventoryManager.Instance.HoverObject.transform.GetChild (0).GetComponent<Text> ().text = InventoryManager.Instance.MovingSlot.Items.Count > 1 ? InventoryManager.Instance.MovingSlot.Items.Count.ToString () : string.Empty;
	}

	private void PutItemBack () { // put item back to the inventory when inventory closing while having the hover
		if (InventoryManager.Instance.From != null) {
			Destroy (GameObject.Find ("Hover")); // destroys the hovericon
			InventoryManager.Instance.From.GetComponent<Image> ().color = Color.white;
			InventoryManager.Instance.From = null;
		} 
		else if (!InventoryManager.Instance.MovingSlot.IsEmpty) {
			Destroy (GameObject.Find ("Hover"));
			foreach (ItemScript item in InventoryManager.Instance.MovingSlot.Items) {
				InventoryManager.Instance.Clicked.GetComponent<Slot>().AddItem(item);
			}
			InventoryManager.Instance.MovingSlot.ClearSlot();
		}
		InventoryManager.Instance.selectStackSize.SetActive (false);
	}

	public void SplitStack () {
		InventoryManager.Instance.selectStackSize.SetActive (false);
		InventoryManager.Instance.tooltipObject.SetActive (false);
		if (InventoryManager.Instance.SplitAmount == InventoryManager.Instance.MaxStackCount) {
			MoveItem(InventoryManager.Instance.Clicked);
		}
		else if (InventoryManager.Instance.SplitAmount > 0) {
			InventoryManager.Instance.MovingSlot.Items = InventoryManager.Instance.Clicked.GetComponent<Slot>().RemoveItems(InventoryManager.Instance.SplitAmount);
			CreateHoverIcon();
		}
	}

	public void ChangeStackText (int i) {
		InventoryManager.Instance.SplitAmount += i;
		if (InventoryManager.Instance.SplitAmount < 0) {	
			InventoryManager.Instance.SplitAmount = 0;
		}
		if (InventoryManager.Instance.SplitAmount > InventoryManager.Instance.MaxStackCount) {
			InventoryManager.Instance.SplitAmount = InventoryManager.Instance.MaxStackCount;
		}
		InventoryManager.Instance.stackText.text = InventoryManager.Instance.SplitAmount.ToString ();
	}

	public void MergeStacks (Slot source, Slot destination) {
		int max = destination.CurrentItem.Item.MaxSize - destination.Items.Count;
		int count = source.Items.Count < max ? source.Items.Count : max;
		for (int i = 0; i < count; i++) {
			destination.AddItem(source.RemoveItem());
			InventoryManager.Instance.HoverObject.transform.GetChild (0).GetComponent<Text> ().text = InventoryManager.Instance.MovingSlot.Items.Count.ToString();	
		}
        if (source.Items.Count == 0) //We ont have more items to merge with
        {
            //FIX REMOVES SOURCE.CLEAR
            Destroy(GameObject.Find("Hover"));
        }
    }

	protected virtual  IEnumerator FadeOut () {
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

                if (instantClose) {
                    break;
                }

				yield return null;
			}

			itemGroup.alpha = 0;
            instantClose = false;
			fadingOut = false;
		}
	}

    private IEnumerator FadeIn() {
        if (!fadingIn) {
            fadingOut = false;
            fadingIn = true;
            StopCoroutine("FadeOut");

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

    public void OpenInventorySound() {
        Instantiate(soundToOpen, transform.position, transform.rotation);
        Destroy(GameObject.Find("OpenInventorySound(Clone)"), lifeTimeSound);
    }
}
















