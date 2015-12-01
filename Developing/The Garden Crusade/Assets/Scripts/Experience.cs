using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Experience : MonoBehaviour {

    public float currentExp;
    public GameObject expBar;
    public int currentLevel;
    public int maxLevel;
    public float expGet;

	void Start () {
        
	}
	

	void Update () {
        expBar.GetComponent<Image>().fillAmount = currentExp;

        if(currentExp >= 1 && maxLevel <10)
        {
            GetComponent<PlayerScript>().baseAgility += 5;
            GetComponent<PlayerScript>().baseIntellect += 5;
            GetComponent<PlayerScript>().baseStamina += 5;
            GetComponent<PlayerScript>().baseStrength += 5;
            currentLevel += 1;
            maxLevel += 1;
            currentExp = 0;
            expGet /= 1.25f;
        }
    }

}
