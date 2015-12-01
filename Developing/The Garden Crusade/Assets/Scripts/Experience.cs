using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Experience : MonoBehaviour {

    public float currentExp;
    public GameObject expBar;
    public int currentLevel;
    public int maxLevel;

	void Start () {
        
	}
	

	void Update () {
        expBar.GetComponent<Image>().fillAmount = currentExp;

        if(currentExp >= 1 && maxLevel <10)
        {
            currentLevel += 1;
            maxLevel += 1;
            currentExp = 0;
        }
    }

}
