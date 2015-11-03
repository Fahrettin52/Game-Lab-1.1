 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    private static PlayerScript instance;

    public static PlayerScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerScript>();
            }
            return instance;
        }
    }

    public CanvasGroup character;
    public float speed;
	public Inventory inventory;
	private Inventory chest;
	public RectTransform healthTransform;
	public float cachedY;
	private float minXValue;
	private float maxXValue;
	public int currentHealth;
	public Image mana;
	public int healtPot;
    public GameObject sarah;

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
	
	void Start (){
		cachedY = healthTransform.position.y;

		maxXValue = healthTransform.position.x;

		minXValue = healthTransform.position.x - healthTransform.rect.width;

		currentHealth = maxHealth;

		onCooldown = false;
	}

    void Update()
    {
        HandleMovement();

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.Open();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (chest != null)
            {
                chest.Open();
            }
        }
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    if(character.alpha == 0)
        //    {
        //        character.alpha = 1;
        //    }
        //    else
        //    {
        //        character.alpha = 0;
        //    }

        //}
    }

	public void HandleHealth (){
		healthText.text = "Health: " + currentHealth; 

		float currentXValue = MapValues (currentHealth, 0, maxHealth, minXValue, maxXValue);

		healthTransform.position = new Vector3 (currentXValue, cachedY);

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
        if (other.gameObject.name == "Cube")
        { 
            Application.LoadLevel(2);
        }

        if (other.tag == "Generator" || other.tag == "DroppedItem") {
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
		if (other.tag == "Chest") {
			chest = other.GetComponent<ChestScript>().chestInventory;  
		}
        Destroy(GameObject.FindGameObjectWithTag("DroppedItem"));
    }

	private void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Chest") {
			if (chest.IsOpen) {
				chest.Open ();
			}
			chest = null;
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            if (inventory.AddItem(collision.gameObject.GetComponent<ItemScript>()))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void GetHealth () {
		currentHealth += healtPot;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		HandleHealth ();
	}
} 












