using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    public GameObject Keypad;
    public GameObject Player;
    public GameObject Door;
    
    [SerializeField]
    private int[] Answer = new int[4];      // 키패드의 정답 변수
    int[] keyNumber = new int[4];   // 사용자 입력 변수
    int value=0;                    // 버튼 입력 횟수
    int AnswerNumber=0;             // 정답 횟수

    // 키패드 UI를 출력하는 메소드
    public void KeyPadUI(){
        GameManager.instance.MouseClip(false);
        Keypad.SetActive(true);
    }

    // 정답 확인 메소드
    public void AnswerCheck(){
        if (value == 4){
            for(int i=0; i<4; i++){
                if ( keyNumber[i] == Answer[i] ){
                    AnswerNumber++;
                    Debug.Log("정답");
                }
                else{
                    Debug.Log("오답");
                } 
                Debug.Log(Answer[i] + ", " + keyNumber[i]);
            }
        }

        PlayerInterface();
        AnswerNumber = 0;
    }

    private void PlayerInterface(){
        if (AnswerNumber == 4){
            DoorController doorCon = Door.GetComponent<DoorController>();
            doorCon.doorLock = false;
            gameObject.SetActive(false);
            doorCon.doorEvent();
        }
        PlayerController PlayerCon = Player.GetComponent<PlayerController>();
        
        GameManager.instance.MouseClip(true);
        Keypad.SetActive(false);
        PlayerCon.hasStop = true;

        value = 0;
        AnswerNumber = 0;
    }

    // 버튼의 숫자값을 저장하는 메소드
    public void KeyCheck(int buttonIndex){
        if (value < 4){
            Debug.Log(buttonIndex + ", " + value);
            keyNumber[value] = buttonIndex;
            value++;
        }
    }

    // 빨간 버튼 누를 시 메소드
    public void KeyMin(){
        if (value > -1){
            keyNumber[value] = -1;
            if (keyNumber[0] != -1) value--;
        }
    }
}
