using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QualitySetting : MonoBehaviour {

    public int QualityChange;
    public string[] TextShown;

    void Start()
    {
        QualitySettings.SetQualityLevel(QualityChange);
        TextChange();
    }

    void Update(){
        QualitySettings.SetQualityLevel(QualityChange);
        TextChange();
    }
    public void TextChange() {
        if (GetComponent<ToSceneOne>().ActivateCanvas == true)
        {
            GameObject.Find("_Manager/OptionScreen/QualityNow").GetComponent<Text>().text = TextShown[QualityChange];
        }
    }
    public void qualityUp(){
        if(QualityChange < 6 && QualityChange >= 0 && QualityChange < 5) {
            QualityChange += 1;
        }
    }
    public void qualityDown(){
        if(QualityChange <= 6 && QualityChange > 0){
            QualityChange -= 1;
        }
    }
}
