using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class Stamina : MonoBehaviour {
	
	public Image bar;
	public float staminaCost;
	public Text staminaText;
	public float maxStamina;
	public float currentStamina;
	public float StaminaRegen;
	public bool hitCooldown;
	public float cooldown;


	void Start () {
		currentStamina = maxStamina;
		hitCooldown = false;
	}

	void Update () {
		ManaText ();
		ManaRegen ();
		ManaColor ();
		if (Input.GetButtonDown ("Fire1") && !hitCooldown && Inventory.ItemGroup.alpha <1) {
			hitCooldown = true;
			ManaDrop ();
			StartCoroutine(CoolDownDmg());
		}
	}

	public void ManaDrop(){
		if (hitCooldown == true) {
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
		staminaText.text = "Stamina: " + currentStamina.ToString("F0"); 
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
}
















