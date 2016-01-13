using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS : MonoBehaviour {
    public float updateInterval = 1.0f;
    public Color textColor = new Color(1, 1, 1, 1);

    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;

    void Start() {
        if (!GetComponent<Text>()) {
            enabled = false;
            return;
        }
        timeleft = updateInterval;
    }

    void Update() {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0) {
            Text tempText = GetComponent<Text>();
            tempText.text = "FPS: " + (accum / frames).ToString("F0");
            tempText.color = textColor;
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}