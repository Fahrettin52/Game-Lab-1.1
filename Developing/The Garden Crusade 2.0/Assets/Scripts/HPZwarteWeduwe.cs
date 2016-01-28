using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPZwarteWeduwe : MonoBehaviour
{

    public Transform enemy;
    public RectTransform bar;
    public Transform player;
    public Camera mainCamera;
    public Text hpText;
    public GameObject weduwe;

    void Start() {
        
        
    }

    void Update()
    {
        if(GameObject.Find("Player").GetComponent<Movement>().secondMode == false){
         mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        }
        
        player = GameObject.Find("Player").GetComponent<Transform>();
        if (player != null)
        {
            bar.position = enemy.position + new Vector3(0, 2f, 0);
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            hpText.text = "Health: " + weduwe.GetComponent<ZwarteWeduwe>().livesEnemy.ToString("F0");
        }
    }
}
