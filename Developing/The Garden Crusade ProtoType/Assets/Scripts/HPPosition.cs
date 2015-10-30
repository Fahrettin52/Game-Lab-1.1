using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPPosition : MonoBehaviour {

    public Transform enemy;
    public RectTransform bar;
    public Transform player;
    public Camera mainCamera;
    public Text hpText;
    public GameObject termiet;

    void Update () {
        bar.position = enemy.position + new Vector3(0, 2f, 0);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        hpText.text = "Health: " + termiet.GetComponent<AnimationTermite>().livesEnemy.ToString("F0");
    }
}
