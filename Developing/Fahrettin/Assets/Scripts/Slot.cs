using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<Item> items;
	public Stack<Item> Items {
		get { return items; }
		set { items = value; }
	}

	public Text stackTxt;
	public Sprite slotEmpty;
	public Sprite slotHiglight;

	public bool IsEmpty{
		get { return items.Count == 0; }
	}

	public bool IsAvailabele{
		get { return CurrentItem.maxSize > items.Count; }
	}
	// returns the current item in the stack
	public Item CurrentItem{
		get { return items.Peek (); }
	}
	
	void Awake () {
		items = new Stack <Item> (); // instatiates the item stacks
	}
	
	void Start () {
		RectTransform slotRect = GetComponent<RectTransform> (); // creates a reference to the slot slots recttransform
		RectTransform txtRect = stackTxt.GetComponent<RectTransform> (); // creates a reference to the stacktexts recttransform

		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.55); // calculates the scalefactor of the text by taking 55 % of the sloths width

		stackTxt.resizeTextMaxSize = txtScaleFactor; // set the min and max textsize of the stacktext
		stackTxt.resizeTextMinSize = txtScaleFactor; // set the min and max textsize of the stacktext

		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x); // set the actual size of the textrect
		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);// set the actual size of the textrect
	}
	// adds a single item to the inventory 
	public void AddItem (Item item) {
		items.Push (item); // adds the item to the stack

		if (items.Count > 1) { // check if we have a stacked item
			stackTxt.text = items.Count.ToString(); // if the item is stacked than we need to write the stack number on top of it
		}

		ChangeSprite (item.spriteNeutral, item.spriteHighlighted); 
	}

	private void UseItem () {
		if (!IsEmpty) {
			items.Pop ().Use ();
			stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
			
			if (IsEmpty){
				ChangeSprite(slotEmpty, slotHiglight); 
				Inventory.EmptySlots++;
			}
		}
	}

	private void ChangeSprite (Sprite neutral, Sprite highlight) {
		GetComponent <Image>().sprite = neutral;

		SpriteState st = new SpriteState ();
		st.highlightedSprite = highlight;
		st.pressedSprite = neutral;

		GetComponent<Button> ().spriteState = st;
	}

	public void AddItems (Stack <Item> items) {

		this.items = new Stack <Item> (items);

		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

		ChangeSprite (CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted );
	}

	public void ClearSlot () {	
		items.Clear (); // clears all items on the slot
		ChangeSprite (slotEmpty, slotHiglight); // change sprite to empty
		stackTxt.text = string.Empty; // clear the text
	}
	 
	public Stack<Item> RemoveItems(int amount){
		Stack<Item> tmp = new Stack<Item>();
		for (int i = 0; i <amount; i++) {
			tmp.Push(items.Pop());
		}
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty; // if its larger than 1 than items count if not string empty

		return tmp;
	}

	public Item RemoveItem () {
		Item tmp;
		tmp = items.Pop ();
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		return tmp;
	}

	public void OnPointerClick (PointerEventData eventData) { // handles event pointer
		if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("Hover") && Inventory.ItemGroup.alpha >0) { // if right button is clicked and our hover item doesnt excist this way you cant use item during dragging
			UseItem(); // uses an item on the slot	
		}
		else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !IsEmpty && !GameObject.Find("Hover")){ // if i click the left mouse button and the same time holding leftshift and the slot has items on it and im not moving any items 	
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(Inventory.Instance.canvas.transform as RectTransform, Input.mousePosition, Inventory.Instance.canvas.worldCamera, out position);
			Inventory.Instance.selectStackSize.SetActive(true);
			Inventory.Instance.selectStackSize.transform.position = Inventory.Instance.canvas.transform.TransformPoint(position);
			Inventory.Instance.SetStackInfo(items.Count); // telling the inventory that we have a x amount off slots that you are alowed to remove
			Inventory.Instance.tooltipObject.SetActive(false);
		}
	}
}














