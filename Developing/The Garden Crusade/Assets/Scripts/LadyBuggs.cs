using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LadyBuggs : MonoBehaviour
{

    public enum Direction { One, Two, Three, Four }; 
    public Direction myDirection;
    public Sprite ladybugSlot1;


    void Start()
    {
        GetComponent<Image>().sprite = ladybugSlot1;
    }
}
