using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locker : MonoBehaviour
{
    public DoorController lockerObejct;     // 금고 문 오브젝트
    public GameObject lockerPrint;          // 금고 출력 오브젝트
    public GameObject itemObject;                 // 금고안에 있는 아이템

    public InputField[] InputNumber = new InputField[4];    // 입력 패스워드
    public char[] PW = new char[4];                           // 정답 패스워드

    public Button button_Check;     // 정답 버튼
    public Text wrongAnswerText;    // 오답 Text

    int answer=0;                   // 정답 개수
    bool hasEnd = false;            // Update의 힘을 덜어주기 위한 변수

    void Update() {
        if (InputNumber[PW.Length-1].isFocused == true && !hasEnd){   // 마지막 패스워드가 입력됐을 때
            if (Input.GetKeyDown(KeyCode.KeypadEnter)){     // Enter를 누를 시 
                Locker_PWCheck();                           // Locker_PWCheck() 호출
            }
        }
    }

    // 비밀번호 체크
    public void Locker_PWCheck(){
        for(int i=0; i<PW.Length; i++){
            if (InputNumber[i].text == PW[i].ToString()){   // 패스워드 비교
                answer++;
                Debug.Log(i + " : 정답");
            }
            else{
                Debug.Log(i + " : 오답");
            }
        }

        if (answer == PW.Length){
            hasEnd = true;
            gameObject.SetActive(false);                    // UI 비활성화
            lockerPrint.SetActive(false);                   // 오브젝트 비활성화
            GameManager.instance.PlayerStop(false);         // Player 움직임 활성화

            lockerObejct.doorLock = false;                  // 문 잠금 해제
            lockerObejct.hasDoor = !lockerObejct.hasDoor;   // 문 열기

            itemObject.SetActive(true);
        }
        else{
            wrongAnswerText.gameObject.SetActive(true);     // 오답 text 출력
        }
        answer = 0; // 정답 개수 초기화
    }
}
