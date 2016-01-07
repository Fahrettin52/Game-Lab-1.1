using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPPosWalk : MonoBehaviour {

    public Transform enemy;
    public RectTransform bar;
    public Transform player;
    public Camera mainCamera;
    public Text hpText;
    public GameObject termiet;

    void Start() {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update () {
        if (player != null)
        {
            bar.position = enemy.position + new Vector3(0, 2f, 0);
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            hpText.text = "Health: " + termiet.GetComponent<AnimationWalkTermite>().livesEnemy.ToString("F0");
        }
    }
}
