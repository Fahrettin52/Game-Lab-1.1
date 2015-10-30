using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class Stamina : MonoBehaviour {

    private static Stamina instance;

    public static Stamina Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Stamina>();
            }
            return instance;
        }
    }

    public GameObject sarah;
    public Image bar;
	public float staminaCost;
	public Text staminaText;
	public float maxStamina;
	public float currentStamina;
	public float StaminaRegen;
	public bool hitCooldown;
	public float cooldown;
	public int energyPot;
	private CanvasGroup itemGroup;
    public int rayDistance;
    public RaycastHit rayHit;
    public int damagePunch;
    public GameObject enemy;


    void Start () {
		itemGroup = GameObject.Find ("InventoryBackground").GetComponent<CanvasGroup> ();
		currentStamina = maxStamina;
		hitCooldown = false;
	}

	void Update () {
		ManaText ();
		ManaRegen ();
		ManaColor ();

        if (Input.GetButtonDown("Fire1") && !hitCooldown && itemGroup.alpha < 1 && !Inventory.mouseInside)
            
        {
            StartCoroutine(FightToIdle());
            hitCooldown = true;
            ManaDrop();
            StartCoroutine(CoolDownDmg());

            if (Physics.Raycast(transform.position + new Vector3(0,1.3f,0), transform.forward, out rayHit, rayDistance))
            {
                Debug.DrawRay(transform.position + new Vector3(0,1.3f,0), transform.forward, Color.green, rayDistance);
                if (rayHit.transform.tag == "Enemy")
                {
                    print("Enemy");
                    enemy.GetComponent<AnimationTermite>().DropDead(damagePunch); 

                }
            }
        }
	}

    IEnumerator FightToIdle()
    {
        yield return new WaitForSeconds(2);
        sarah.GetComponent<AnimationSara>().fightToIdle();
    }

    public void ManaDrop(){
		if (hitCooldown == true) {
            sarah.GetComponent<AnimationSara>().SaraPunch();
            bar.fillAmount -= staminaCost;
			currentStamina -= staminaCost*100;
		}
		if (currentStamina <= 0) {
			currentStamina =0;
		}
	}

	public void ManaRegen(){
		if (currentStamina < maxStamina) {
			bar.fillAmount += StaminaRegen * Time.deltaTime;
			currentStamina += StaminaRegen*100 * Time.deltaTime;
		}
		if (currentStamina > maxStamina) {
			currentStamina = maxStamina;
		}
	}

	public void ManaText (){
		staminaText.text = "Energy: " + currentStamina.ToString("F0"); 
	}

	public void ManaColor(){

		if (currentStamina > maxStamina / 2 ) { 
			bar.color = new Color32 (0, (byte)MapValues (currentStamina, maxStamina / 2, maxStamina, 255, 0), 255, 255);
		}
		else { 
			bar.color = new Color32 ((byte)MapValues (currentStamina, 0, maxStamina / 2 , 255, 0), 255, 255, 255);
		}
	}

	private float MapValues(float x, float inMin, float inMax, float outMin, float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin; 
	}

	IEnumerator CoolDownDmg (){
		hitCooldown = true;
		yield return new WaitForSeconds (cooldown);
        hitCooldown = false;
	}
	public void GetMana () {
		currentStamina += energyPot;
		bar.fillAmount += energyPot / 100f;
		if (currentStamina > maxStamina) {
			currentStamina = maxStamina;
		}
		ManaColor ();
		ManaText ();
	}
}
















