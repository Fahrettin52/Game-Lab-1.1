using UnityEngine;
using System.Collections;
using System;

public class CharacterPanel : Inventory {

    public Slot[] equipmentSlots;

    private static CharacterPanel instance;

    public static CharacterPanel Instance
    {
        get {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<CharacterPanel>();
            }
            return CharacterPanel.instance; }
    }

    public Slot WeaponSlot
    {
        get { return equipmentSlots[9]; }
    }

    public Slot OffhandSlot
    {
        get { return equipmentSlots[10]; }
    }

    void Awake() {
        equipmentSlots = transform.GetComponentsInChildren<Slot>();
    }

    public override void CreateLayout()
    {
       
    }

    void Start () {
        itemGroup = GetComponentInParent<CanvasGroup>();
        buttonGroup = GameObject.Find("ButtonCanvas").GetComponent<CanvasGroup>();
    }

    public void EquipItem(Slot slot, ItemScript item){

        if (item.Item.ItemType == ItemType.MAINHAND || item.Item.ItemType == ItemType.TWOHAND && OffhandSlot.IsEmpty)
        {
            Slot.SwapItems(slot, WeaponSlot);
        }
        else
        {
            Slot.SwapItems(slot, Array.Find(equipmentSlots, x => x.canContain == item.Item.ItemType));
        }
    }

    public override void ShowToolTip(GameObject slot)
    {
        Slot tmpSlot = slot.GetComponent<Slot>();
        if (!tmpSlot.IsEmpty && InventoryManager.Instance.HoverObject == null && !InventoryManager.Instance.selectStackSize.activeSelf && slot.GetComponentInParent<Inventory>().IsOpen)
        {
            InventoryManager.Instance.visualTextObject.text = tmpSlot.CurrentItem.GetToolTip();
            InventoryManager.Instance.SizeTextObject.text = InventoryManager.Instance.visualTextObject.text;
            InventoryManager.Instance.tooltipObject.SetActive(true);
            InventoryManager.Instance.tooltipObject.transform.position = slot.transform.position;
        }
    }
}
