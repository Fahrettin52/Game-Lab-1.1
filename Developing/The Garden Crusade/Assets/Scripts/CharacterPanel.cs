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

    public void CalculateStats()
    {
        int strength = 0;
        int stamina = 0;
        int intellect = 0;
        int agility = 0;

        foreach (Slot slot in equipmentSlots)
        {
            if (!slot.IsEmpty)
            {
                Equipment e = (Equipment)slot.CurrentItem.Item;
                strength    += e.Strength;
                stamina     += e.Stamina;
                intellect   += e.Intellect;
                agility     += e.Agility;
            }
        }

        PlayerScript.Instance.SetStats(strength, stamina, intellect, agility);
    }

    public override void SaveInventory()
    {
        print("Stats Save");
        string content = string.Empty;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (!equipmentSlots[i].IsEmpty)
            {
                content += i + "-" + equipmentSlots[i].Items.Peek().Item.ItemName + ";";
            }
        }

        PlayerPrefs.SetString("CharacterBackground", content);
        PlayerPrefs.Save();
    }

    public override void LoadInventory()
    {
        print("Stats Load");
        foreach (Slot slot in equipmentSlots)
        {
            slot.ClearSlot();
        }

        string content = PlayerPrefs.GetString("CharacterBackground");
        string[] splitContent = content.Split(';');

        for (int i = 0; i < splitContent.Length - 1; i++)
        {
            string[] splitValues = splitContent[i].Split('-');

            int index = Int32.Parse(splitValues[0]);
            string itemName = splitValues[1];

            GameObject loadedItem = Instantiate(InventoryManager.Instance.itemObject);

            loadedItem.AddComponent<ItemScript>();

            if (index == 9 || index == 10)
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContainer.Weapons.Find(x => x.ItemName == itemName);
            }
            else
            {
                loadedItem.GetComponent<ItemScript>().Item = InventoryManager.Instance.ItemContainer.Equipment.Find(x => x.ItemName == itemName);
            }

            equipmentSlots[index].AddItem(loadedItem.GetComponent<ItemScript>());

            Destroy(loadedItem);

            CalculateStats();
        }
    }
}
 