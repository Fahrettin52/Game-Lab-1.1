using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastingBar : MonoBehaviour {

	public Vector3 startPos;
	public Vector3 endPos;
	public Image castImage;
	public RectTransform castTransform;
	public Canvas canvas;
	private Spell dubbleHit = 		new Spell("Dubble Hit", 1f, Color.blue);
	private Spell trippleHit = 		new Spell("Tripple Hit", 1.5f, Color.red);
	private Spell spinningCombo = 	new Spell("Spinning", 3f, Color.black);
	private Spell heal = 			new Spell("Heal", 1f, Color.green);

    public int rayDistance;
    public RaycastHit rayHit;
    public int damageSpell1;
    public int damageSpell2;
    private bool casting;

	public Text castTime;
	public Text spellName;

	public CanvasGroup spellGroup;
	public float fadeSpeed;
    private GameObject player;

    public float radiusSpell;
    public Vector3 direction;
    public float useSpeed;
    public float useSpeedReset;
    public int damage;

    void Start () {

        player = GameObject.Find("Player");
        casting = false;
		//castTransform = GetComponent<RectTransform>();
		//castImage = GetComponent<Image>();
		endPos = castTransform.position;
		startPos = new Vector3 (castTransform.position.x - (castTransform.rect.width * canvas.scaleFactor), castTransform.position.y, castTransform.position.z);
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1) && player != null) {
            StartCoroutine(CastSpell(dubbleHit));
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDistance)) {
                if (rayHit.transform.tag == "Enemy") {
                    rayHit.transform.GetComponent<AnimationTermite>().DropDead(damageSpell1);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && player != null) {
            StartCoroutine(CastSpell(trippleHit));
            if (Physics.Raycast(transform.position + new Vector3(0, 1.3f, 0), transform.forward, out rayHit, rayDistance)) {
                if (rayHit.transform.tag == "Enemy") { 
                    rayHit.transform.GetComponent<AnimationTermite>().DropDead(damageSpell2);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && player != null) {
            AreaDamage();
            StartCoroutine(CastSpell(spinningCombo));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && player != null) {
            StartCoroutine(CastSpell(heal));
            GetComponent<PlayerScript>().currentHealth += 10;
            GetComponent<PlayerScript>().GetHealth();
        }
    }

	private IEnumerator FadeOut () {

		StopCoroutine ("FadeIn");

		while (spellGroup.alpha > 0f) {

			float newValue = fadeSpeed * Time.deltaTime;

			if ((spellGroup.alpha - newValue) > 0f) {
				spellGroup.alpha -= newValue;
			}

			else { 
				spellGroup.alpha = 0;
			}
			yield return null;	 
		}
	}

	private IEnumerator FadeIn () {

		StopCoroutine ("FadeOut");
		
		while (spellGroup.alpha < 1f) {
			
			float newValue = fadeSpeed * Time.deltaTime;
			
			if ((spellGroup.alpha + newValue) < 1f) {
				spellGroup.alpha += newValue;
			}
			
			else { 
				spellGroup.alpha = 1;
			}
			yield return null;	 
		}
	}

	private IEnumerator CastSpell (Spell spell){

		if (!casting) {
			StartCoroutine("FadeIn");
			casting = true;
			castImage.color = spell.Spellcolor;
			castTransform.position = startPos ;
			float timeLeft = Time.deltaTime;
			float rate = 1.0f / spell.CastTime;
			float progress = 0.0f;
			spellName.text = spell.Name;
			
			while (progress <= 1.0) {
                //if (player == null)
                //{
                //    Destroy(GameObject.Find("Canvas"));
                //}
                castTransform.position = Vector3.Lerp (startPos, endPos, progress);
				progress += rate * Time.deltaTime; 
				timeLeft += Time.deltaTime;
				castTime.text = timeLeft.ToString("F1") + " / " + spell.CastTime.ToString("F1") + " sec";
				yield return null;
			}
			castTime.text = spell.CastTime.ToString("F1") + " / " + spell.CastTime.ToString("F1") + " sec";
			castTransform.position = endPos; 
			casting = false;
			StartCoroutine("FadeOut");
        }
	}

    void AreaDamage(){
        RaycastHit[] areaDamage = Physics.SphereCastAll(transform.position, radiusSpell, direction, radiusSpell);
        useSpeed -=1 * Time.deltaTime;
        print(areaDamage.Length);
        if (useSpeed <= 1){
            useSpeed = useSpeedReset;
            for (int i = 0; i < areaDamage.Length; i++){
                if (areaDamage[i].transform.tag == "Enemy"){
                    areaDamage[i].transform.GetComponent<AnimationTermite>().DropDead(damage);
                }
            }
        }
    }
}
