using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LadyBuggs : MonoBehaviour
{
    public Sprite ladybugImage;

    public enum Direction { One, Two, Three, Four }; 
    public Direction myDirection;

    public void Start() {
        GetComponent<Image>().sprite = ladybugImage;
    }

}
