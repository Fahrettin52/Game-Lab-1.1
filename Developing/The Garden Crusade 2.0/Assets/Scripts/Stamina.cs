using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stamina : MonoBehaviour {

    private static Stamina instance;

    public static Stamina Instance
    {
        get
        {
            if (instance == null) {
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
    public CanvasGroup itemGroup;
    public int rayDistance;
    public RaycastHit rayHit;
    public int damagePunch;
    public GameObject enemy;

    public float currentRage;
    public float maxRage;
    public Image rageFillAmount;
    public Text rageText;
    public GameObject soundToOpen;
    public float lifeTimeSound;
    public GameObject soundToOpenDamage;


    void Start() {
        //itemGroup = GameObject.Find ("InventoryBackground").GetComponent<CanvasGroup> ();
        currentStamina = maxStamina;
        hitCooldown = false;
    }

    void Update() {
        ManaText();
        ManaRegen();
        ManaColor();

        if (Input.GetButtonDown("Fire1") && !hitCooldown && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && currentStamina >= 10) {
            StartCoroutine(FightToIdle());
            hitCooldown = true;
            ManaDrop();
            StartCoroutine(CoolDownDmg());
            DamageGivesound();

            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDistance) ||
               (Physics.Raycast(transform.position + new Vector3(0, 0, 0), transform.forward, out rayHit, rayDistance) ||
               (Physics.Raycast(transform.position + new Vector3(0, 2.6f, 0), transform.forward, out rayHit, rayDistance)))) {
                Debug.DrawRay(transform.position + new Vector3(0, 1.3f, 0), transform.forward, Color.green, rayDistance);
                if (rayHit.transform.tag == "Enemy") {
                    rayHit.transform.GetComponent<AnimationTermite>().DropDead(damagePunch);
                }
                 if (rayHit.transform.tag == "Generaal") {
                    rayHit.transform.GetComponent<TermiteGeneral>().DropDead(damagePunch);
                }
                if (rayHit.transform.tag == "EnemyWalk") {
                    rayHit.transform.GetComponent<AnimationWalkTermite>().DropDead(damagePunch);
                }
            }
        }
    }

    IEnumerator FightToIdle() {
        yield return new WaitForSeconds(2);
        sarah.GetComponent<AnimationSara>().fightToIdle();
    }

    public void ManaDrop() {
        if (hitCooldown == true) {
            sarah.GetComponent<AnimationSara>().SaraPunch();
            bar.fillAmount -= staminaCost;
            currentStamina -= staminaCost * 100;
        }
        if (currentStamina <= 0) {
            currentStamina = 0;
        }
    }

    public void ManaRegen() {
        if (currentStamina < maxStamina) {
            bar.fillAmount += StaminaRegen * Time.deltaTime;
            currentStamina += StaminaRegen * 100 * Time.deltaTime;
        }
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    public void ManaText() {
        staminaText.text = "Energy: " + currentStamina.ToString("F0");
    }

    public void ManaColor() {

        if (currentStamina > maxStamina / 2) {
            bar.color = new Color32(0, (byte)MapValues(currentStamina, maxStamina / 2, maxStamina, 255, 0), 255, 255);
        } else {
            bar.color = new Color32((byte)MapValues(currentStamina, 0, maxStamina / 2, 255, 0), 255, 255, 255);
        }
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax) {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    IEnumerator CoolDownDmg() {
        //hitCooldown = true;
        yield return new WaitForSeconds(cooldown);
        hitCooldown = false;
    }
    public void GetMana() {
        currentStamina += energyPot;
        bar.fillAmount += energyPot / 100f;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
        ManaColor();
        ManaText();
    }

    public void RageBar() {
        if (currentRage < maxRage) {
            currentRage += 15f;
        }
        if (currentRage >= maxRage) {
            currentRage = maxRage;           
        }
        rageFillAmount.fillAmount += 1.5f / 10f;

        if (currentRage < maxRage / 2) {
            rageFillAmount.color = new Color32(255, (byte)MapValues(currentRage, maxRage / 2, maxRage, 255, 0), 255, 65);
        } else {
            rageFillAmount.color = new Color32(255, 0, (byte)MapValues(currentRage, 100, maxRage / 2, 0, 255), 130);
        }
        rageText.text = "Rage: " + currentRage.ToString("F0");
    }

    public void RageBarLose(int cost) {
        if (currentRage >= 0) {
            rageFillAmount.fillAmount -= cost / 10f;
            if (currentRage < maxRage / 2) {
                rageFillAmount.color = new Color32(255, (byte)MapValues(currentRage, maxRage / 2, maxRage, 255, 0), 255, 65);
            } else {
                rageFillAmount.color = new Color32(255, 0, (byte)MapValues(currentRage, 100, maxRage / 2, 0, 255), 130);
            }
            rageText.text = "Rage: " + currentRage.ToString("F0");
        }
    }
    public void RagePotion() {
        if (currentRage < 100) {
            currentRage += 10f;
            rageFillAmount.fillAmount += 1f / 10f;
            if (currentRage < maxRage / 2) {
                rageFillAmount.color = new Color32(255, (byte)MapValues(currentRage, maxRage / 2, maxRage, 255, 0), 255, 65);
            } else {
                rageFillAmount.color = new Color32(255, 0, (byte)MapValues(currentRage, 100, maxRage / 2, 0, 255), 130);
            }
            rageText.text = "Rage: " + currentRage.ToString("F0");
        }
    }
    public void RageBarFullSound() {
        Instantiate(soundToOpen, transform.position, transform.rotation);
        Destroy(GameObject.Find("RageBarFullSound(Clone)"), lifeTimeSound);
    }

    public void DamageGivesound() {
        Instantiate(soundToOpenDamage, transform.position, transform.rotation);
        Destroy(GameObject.Find("PlayerDealingDamageSound(Clone)"), lifeTimeSound);
    }
}
















