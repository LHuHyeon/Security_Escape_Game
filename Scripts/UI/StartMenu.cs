using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject startButton;
    public GameObject speakObject;
    public GameObject playerObject;
    public GameObject manualImg;
    public GameObject manualImg2;
    public GameObject manualObj;

    public void StartButton(){
        Speak speakCon = speakObject.GetComponent<Speak>();
        Color color = gameObject.GetComponent<Image>().color;
        PlayerController playCon = playerObject.GetComponent<PlayerController>();

        for(int i=0; i<3; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // for(int i=255; i>=0; i--){
        //     color.a = i;
        //     GetComponent<Image>().color = color;
        // }
        while(color.a > 1){
            color.a = color.a - Time.time;
            GetComponent<Image>().color = color;
        }

        speakCon.StartSpeak();
        GameManager.instance.MouseClip(true);
        GameManager.instance.minhasTime = false;
        gameObject.SetActive(false);
        playCon.hasStart = true;
    }

    public void ManualExit(){           // 메뉴얼 나가기
        manualObj.SetActive(false);
    }

    public void ManualButtion(){        // 메뉴얼 켜기
        manualObj.SetActive(true);
    }
    
    public void ExplanButton1(){        // 1번 메뉴얼
        manualImg.SetActive(true);
        manualImg2.SetActive(false);
    }

    public void ExplanButton2(){        // 2번 메뉴얼
        manualImg.SetActive(false);
        manualImg2.SetActive(true);
        startButton.SetActive(true);    // Start Button true
    }

    public void ExplanButton2_Esc(){
        manualImg.SetActive(false);
        manualImg2.SetActive(true);
    }
}