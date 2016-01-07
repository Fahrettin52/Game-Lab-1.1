using UnityEngine;
using System.Collections;

public class ManagerCheck : MonoBehaviour {

    public static ManagerCheck manager;

    void OnLevelWasLoaded() {
        if (manager == null) {
            manager = this;
        } 
        else if (manager != this) {
            Destroy(gameObject);
        }
    }
}