﻿using UnityEngine;
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

        if(currentExp >= 1 && currentLevel <maxLevel)
        {
            GetComponent<PlayerScript>().baseAgility += 5;
            GetComponent<PlayerScript>().baseIntellect += 5;
            GetComponent<PlayerScript>().baseStamina += 5;
            GetComponent<PlayerScript>().baseStrength += 5;
            currentLevel += 1;
            currentExp = 0;
            expGet /= 1.25f;
            GetComponent<PlayerScript>().SetStats(0, 0, 0, 0);
        }
    }

}
