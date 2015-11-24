using UnityEngine;
using System.Collections;

public class MenuStamina : MonoBehaviour {

    public bool staminaOut;

    public void Start()
    {
        staminaOut = true;
    }

    public void StaminaDisable()
    {
        staminaOut = !staminaOut;
        GameObject.Find("Player").GetComponent<Stamina>().enabled = staminaOut;
    }
}
