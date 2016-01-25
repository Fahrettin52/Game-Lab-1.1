using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    private static PlayerScript instance;

    public static PlayerScript Instance {
        get{
            if (instance == null) {
                instance = FindObjectOfType<PlayerScript>();
            }
            return instance;
        }
    }

    public CanvasGroup inventoryGroup;
    public CanvasGroup characterBackgroundHolder;
    public CanvasGroup character;
    public float speed;
    public Inventory Crafting;
	public Inventory inventory;
	public Inventory chest;
    public Inventory charPanel;
	public RectTransform healthTransform;
	public float cachedY;
	private float minXValue;
	private float maxXValue;
	public int currentHealth;
	public Image mana;
	public int healtPot;
    public GameObject sarah;
    public Text strengthStats, staminaStats, intellectStats, agilityStats;
    [SerializeField]
    private Text goldText;
    public int baseStrength, baseStamina, baseIntellect, baseAgility;
    public int strength, stamina, intellect, agility;
    private int gold;


    public int Gold {
        get { return gold; }
        set {
            goldText.text = "Crumbs: " + value;
            gold = value;
        }
    }

    private int CurrentHealth {
		get	{ return currentHealth; }
		set	{
			currentHealth = value; 
			HandleHealth ();
		}
	}

    public int maxHealth; 
	public Text healthText;
	public Image visualHealth;
	public float cooldown;
	public bool onCooldown;

    public GameObject soundToOpenPickUp;
    public int lifeTimeSound;

    void Start (){
        Gold = 0;
        SetStats(0, 0, 0, 0);
        cachedY = healthTransform.position.y;
		maxXValue = healthTransform.position.x;
		minXValue = healthTransform.position.x - healthTransform.rect.width;
		currentHealth = maxHealth;
		onCooldown = false;
	}

    public void PauseGame() {        
        GameObject.Find("_Manager").GetComponent<ToSceneOne>().OpenOptionsInGame();
        GameObject.Find("_Manager").GetComponent<SoundSource>().MouseClick();
        if (Time.timeScale == 1.0F) {
            Time.timeScale = 0f;
        } 
        else {
            if (Time.timeScale == 0f) {
                Time.timeScale = 1.0f;
            }
        }
    }

    void Update() {
    if (Input.GetButtonDown("Escape")) {
        PauseGame();
    }
        visualHealth.fillAmount = currentHealth / 100f;
        HandleHealth();
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.I)){
            CraftingBench.Instance.UpdatePreview();
            GameObject.Find("InventoryBackground").GetComponent<Inventory>().OpenInventorySound();
            if (inventory.GetComponent<CanvasGroup>().alpha < 1) {
                inventory.GetComponent<CanvasGroup>().blocksRaycasts = true;
            } else {
                inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            if (inventory.GetComponent<CanvasGroup>().alpha == 0 || inventory.GetComponent<CanvasGroup>().alpha == 1) {
                inventory.Open();
            }
            if (Crafting.GetComponent<CanvasGroup>().alpha < 1) {
                Crafting.GetComponent<CanvasGroup>().blocksRaycasts = true;
            } else {
                Crafting.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            if (Crafting.GetComponent<CanvasGroup>().alpha == 0 || inventory.GetComponent<CanvasGroup>().alpha == 1) {
                Crafting.Open();
            }
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            if (chest != null) {
                if (chest.GetComponent<CanvasGroup>().alpha == 0 || chest.GetComponent<CanvasGroup>().alpha == 1) {
                    chest.Open();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            if (characterBackgroundHolder.GetComponent<CanvasGroup>().alpha < 1) {
                characterBackgroundHolder.GetComponent<CanvasGroup>().blocksRaycasts = true;
            } else {
                characterBackgroundHolder.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            if (charPanel != null) {
                if (characterBackgroundHolder.GetComponent<CanvasGroup>().alpha == 0 || characterBackgroundHolder.GetComponent<CanvasGroup>().alpha == 1) {
                    charPanel.Open();
                }
            }
        }
    }

	public void HandleHealth (){
		healthText.text = "Health: " + currentHealth; 

		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY) * InventoryManager.Instance.canvas.scaleFactor;

		if (currentHealth > maxHealth / 2 ) { 
			visualHealth.color = new Color32 ((byte)MapValues (currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
		}
		else { 
			visualHealth.color = new Color32 (255, (byte)MapValues (currentHealth, 0, maxHealth / 2 , 0, 255), 0, 255);
		}
		if (currentHealth <= 0) {
            StartCoroutine(Dead());
            
        }
	}

    IEnumerator Dead()
    {
        sarah.GetComponent<AnimationSara>().mayDie();
        GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        Destroy(GameObject.Find("Canvas"));
        GameObject.Find("_Manager").GetComponent<ToSceneOne>().deadScreen.SetActive(true);
        GameObject.Find("_Manager").GetComponent<AudioSource>().enabled = false;
    }
	
	private float MapValues(float curHealth, float minValue, float maxValue, float outMin, float outMax){
		return (curHealth - minValue) * (outMax - outMin) / (maxValue - minValue) + outMin; 
	}

	IEnumerator CoolDownDmg (){
		onCooldown = true;
		yield return new WaitForSeconds (cooldown);
		onCooldown = false;
	}

	private void HandleMovement (){
		float translation = speed * Time.deltaTime;

		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * translation,0, Input.GetAxis("Vertical") * translation));
	}

	void OnTriggerStay (Collider other){
		if (other.name == "Damage") {
			if(!onCooldown && currentHealth > 0){
				StartCoroutine(CoolDownDmg());
				CurrentHealth -= 1;
			}
		}

		if (other.name == "Regen") {
			if(!onCooldown && currentHealth < maxHealth){
				StartCoroutine(CoolDownDmg());
				CurrentHealth += 1;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
        if (other.tag == "Material"){
            for (int i = 0; i < 5; i++) {
                for (int x = 0; x < 3; x++) {
                    GameObject tmp = Instantiate(InventoryManager.Instance.itemObject);

                    tmp.AddComponent<ItemScript>();

                    ItemScript newMaterial = tmp.GetComponent<ItemScript>();

                    newMaterial.Item = InventoryManager.Instance.ItemContainer.Materials[x];

                    inventory.AddItem(newMaterial);

                    Destroy(tmp);
                }
            }
        }

        if (other.gameObject.tag == "mayThrow") {
        GetComponent<ToThrow>().throwInfo.SetActive(true);
        GetComponent<ToThrow>().throwInfo.GetComponent<Text>().text = GetComponent<ToThrow>().changeText[0];
        }
        if (other.gameObject.name == "ToBoomstronk" && GetComponent<Quests>().quest1[6] == true)
        {
            InventoryManager.Instance.Save();
            Application.LoadLevel("BoomStronk");
        }

        if (other.gameObject.tag == "Load")
        {
            InventoryManager.Instance.Load();
            Destroy(GameObject.Find("LoadPlayerPref"));
        }

        if (other.tag == "Generator" || other.tag == "DroppedItem") {
            PickupSound();
            int randomType = UnityEngine.Random.Range(0,3);
			GameObject tmp = Instantiate(InventoryManager.Instance.itemObject);
			int randomItem;
            tmp.AddComponent<ItemScript>();
            ItemScript newitem = tmp.GetComponent<ItemScript>();

 
            switch (randomType){
			case 0:
					randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Consumable.Count);
                    newitem.Item = InventoryManager.Instance.ItemContainer.Consumable[randomItem];
					break;
					
			case 1:
					randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Weapons.Count);
                    newitem.Item = InventoryManager.Instance.ItemContainer.Weapons[randomItem];
					break;

			case 2:
					randomItem = UnityEngine.Random.Range(0, InventoryManager.Instance.ItemContainer.Equipment.Count);
                    newitem.Item = InventoryManager.Instance.ItemContainer.Equipment[randomItem];
					break;
			}
            inventory.AddItem(newitem);
            Destroy(tmp);
        }
		if (other.tag == "Chest" || other.tag == "Vendor") {
			chest = other.GetComponent<InventoryLink>().linkedInventory;  
		}
        Destroy(GameObject.FindGameObjectWithTag("DroppedItem"));
    }

	private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "mayThrow") {
            GetComponent<ToThrow>().throwInfo.SetActive(false);
        }
        if (other.gameObject.tag == "Chest" || other.gameObject.tag == "Vendor") {
            if (chest.IsOpen) {
                chest.Open();
            }
            chest = null;
        }
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Item") {
            PickupSound();
            if (inventory.AddItem(collision.gameObject.GetComponent<ItemScript>())) {

                Destroy(collision.gameObject);
            }
        }
        if (collision.transform.tag == "Vendor") {
            chest = collision.transform.GetComponent<InventoryLink>().linkedInventory;
        }
    }

    public void GetHealth () {
		currentHealth += healtPot;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		HandleHealth ();
	}

    public void SetStats(int strength, int stamina, int intellect, int agility)
    {
        
        this.strength   = strength + baseStrength;
        this.stamina    = stamina + baseStamina;
        this.intellect  = intellect + baseIntellect;
        this.agility    = agility + baseAgility;

        strengthStats.text  = string.Format("Strength: {0}", this.strength);
        staminaStats.text   = string.Format("Stamina: {0}", this.stamina);
        intellectStats.text = string.Format("Intellect: {0}", this.intellect);
        agilityStats.text   = string.Format("Agility: {0}", this.agility);
    }
    public void PickupSound()
    {
        Instantiate(soundToOpenPickUp, transform.position, transform.rotation);
        Destroy(GameObject.Find("ItemPickupSound(Clone)"), lifeTimeSound);
    }
}









