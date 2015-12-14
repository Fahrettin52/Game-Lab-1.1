using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

	private Stack<ItemScript> items;
	public Text stackTxt;
	public Sprite slotEmpty;
	public Sprite slotHiglight;
	public GameObject player;
	public bool mayUseHealth;
	public bool mayUseMana;
	private CanvasGroup itemGroup;
    public ItemType canContain;

	public Stack<ItemScript> Items {
		get { return items; }
		set { items = value; }
	}

	public bool IsEmpty{
		get { return items.Count == 0; }
	}

	public bool IsAvailabele{
		get { return CurrentItem.Item.MaxSize > items.Count; }
	}

	public ItemScript CurrentItem{
		get { return items.Peek (); }
	}

	void Awake () {
		items = new Stack <ItemScript> (); 
		player = GameObject.FindWithTag ("Player");
	}
	
	void Start () {

		mayUseHealth = false;
		mayUseMana = false;

		RectTransform slotRect = GetComponent<RectTransform> (); 
		RectTransform txtRect = stackTxt.GetComponent<RectTransform> (); 

		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.55); 

		stackTxt.resizeTextMaxSize = txtScaleFactor; 
		stackTxt.resizeTextMinSize = txtScaleFactor; 

		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotRect.sizeDelta.x); 
		txtRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotRect.sizeDelta.y);

		if (transform.parent != null) {

            Transform p = transform.parent;

            while (itemGroup == null && p != null) {
                itemGroup = p.GetComponent<CanvasGroup>();
                p = p.parent;
            }
		}
	}
	
	public void AddItem (ItemScript item) {
        if (IsEmpty){
            transform.parent.GetComponent<Inventory>().EmptySlots--;  
        }

		items.Push (item); 

		if (items.Count > 1) { 
			stackTxt.text = items.Count.ToString(); 
		}

		ChangeSprite (item.spriteNeutral, item.spriteHighlighted); 
	}
	
	public void AddItems (Stack <ItemScript> items) {
		
		this.items = new Stack <ItemScript> (items);
		
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
		
		ChangeSprite (CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted );
	}

	private void ChangeSprite (Sprite neutral, Sprite highlight) {
		GetComponent <Image>().sprite = neutral;
		
		SpriteState st = new SpriteState ();
		st.highlightedSprite = highlight;
		st.pressedSprite = neutral;
		
		GetComponent<Button> ().spriteState = st;
	}
	
	private void UseItem () {
		if (!IsEmpty) {
			items.Peek ().Use(this);
            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

            if (IsEmpty) {
                ChangeSprite(slotEmpty, slotHiglight);
                transform.parent.GetComponent<Inventory>().EmptySlots++;
            }
        }
	}

	public void ClearSlot () {	
		items.Clear (); 
		ChangeSprite (slotEmpty, slotHiglight); 
		stackTxt.text = string.Empty;
    }
	 
	public Stack<ItemScript> RemoveItems(int amount){
		Stack<ItemScript> tmp = new Stack<ItemScript>();
		for (int i = 0; i <amount; i++) {
			tmp.Push(items.Pop());
		}
		stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty; 

		return tmp;
	}

	public ItemScript RemoveItem () {
        if (!IsEmpty) {
            ItemScript tmp;
            tmp = items.Pop();
            stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
            if (IsEmpty) {
                ClearSlot();
            }
            return tmp;
        }
        return null;
	}

	public void OnPointerClick (PointerEventData eventData) { 
		if (eventData.button == PointerEventData.InputButton.Right && !GameObject.Find ("Hover")&& itemGroup.alpha > 0) {
            UseItem ();
        } 
		else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey (KeyCode.LeftShift) && !IsEmpty && !GameObject.Find ("Hover")) { // if i click the left mouse button and the same time holding leftshift and the slot has items on it and im not moving any items 	
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (InventoryManager.Instance.canvas.transform as RectTransform, Input.mousePosition, InventoryManager.Instance.canvas.worldCamera, out position);
			InventoryManager.Instance.selectStackSize.SetActive (true);
			InventoryManager.Instance.selectStackSize.transform.position = InventoryManager.Instance.canvas.transform.TransformPoint (position);
			InventoryManager.Instance.SetStackInfo (items.Count);
			InventoryManager.Instance.tooltipObject.SetActive (false);
		}
	}

    public static void SwapItems(Slot from, Slot to){
        if ( to != null && from != null ){
            bool calculateStats = from.transform.parent == CharacterPanel.Instance.transform || to.transform.parent == CharacterPanel.Instance.transform;

            if (CanSwap(from,to)) {
                    Stack<ItemScript> tmpTo = new Stack<ItemScript>(to.Items);
                    to.AddItems(from.Items);

                    if (tmpTo.Count == 0) {
                        to.transform.parent.GetComponent<Inventory>().EmptySlots--;
                        from.ClearSlot();
                    } 
                    else {
                        from.AddItems(tmpTo);
                    }
            }
            if (calculateStats) {
                CharacterPanel.Instance.CalculateStats();
            }
        }
    }

    public static bool CanSwap(Slot from, Slot to) {

        ItemType fromType = from.CurrentItem.Item.ItemType;
        if (to.canContain == from.canContain) {
            return true;
        }
        if (fromType != ItemType.OFFHAND && to.canContain == fromType) {
            return true;
        }
        if (to.canContain == ItemType.GENERIC && (to.IsEmpty || to.CurrentItem.Item.ItemType == fromType)){
            return true;
        }
        if (fromType == ItemType.MAINHAND && to.canContain == ItemType.GENERICWEAPON) {
            return true;
        }
        if (fromType == ItemType.TWOHAND && to.canContain == ItemType.GENERICWEAPON && CharacterPanel.Instance.OffhandSlot.IsEmpty) {
            return true;
        }
        if (fromType == ItemType.OFFHAND && (to.IsEmpty || to.CurrentItem.Item.ItemType == ItemType.OFFHAND ) && (CharacterPanel.Instance.WeaponSlot.IsEmpty || CharacterPanel.Instance.WeaponSlot.CurrentItem.Item.ItemType != ItemType.TWOHAND)) {
            return true;
        }
        return false;
    }
}














