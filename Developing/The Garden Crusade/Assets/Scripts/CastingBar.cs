using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CastingBar : MonoBehaviour {

	private Vector3 startPos;
	private Vector3 endPos;
	public Image castImage;
	public RectTransform castTransform;
	public Canvas canvas;
	private Spell dubbleHit = 		new Spell("Dubble Hit", 1f, Color.blue);
	private Spell trippleHit = 		new Spell("Tripple Hit", 1.5f, Color.red);
	private Spell spinningCombo = 	new Spell("Spinning", 3f, Color.black);
	private Spell heal = 			new Spell("Heal", 1f, Color.green);

	private bool casting;

	public Text castTime;
	public Text spellName;

	public CanvasGroup spellGroup;
	public float fadeSpeed;
    private GameObject player;

	void Start () {

        player = GameObject.Find("Player");
        casting = false;
		castTransform = GetComponent<RectTransform>();
		castImage = GetComponent<Image>();
		endPos = castTransform.position;
		startPos = new Vector3 (castTransform.position.x - (castTransform.rect.width * canvas.scaleFactor), castTransform.position.y, castTransform.position.z);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1) && player != null) {
			StartCoroutine(CastSpell(dubbleHit));
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) && player != null) {
			StartCoroutine(CastSpell(trippleHit));
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) && player != null) {
			StartCoroutine(CastSpell(spinningCombo));
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) && player != null) {
			StartCoroutine(CastSpell(heal));
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
}
