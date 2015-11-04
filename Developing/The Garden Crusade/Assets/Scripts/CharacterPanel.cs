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
        Slot.SwapItems(slot, Array.Find(equipmentSlots, x => x.canContain == item.Item.ItemType));
    }

}
