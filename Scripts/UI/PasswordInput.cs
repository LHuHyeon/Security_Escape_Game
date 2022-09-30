using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    public GameObject EndingObject;     // 엔딩 오브젝트
    public GameObject monitorObj;       // 모니터 오브젝트
    public InputField inputPassword;    // 입력 패스워드
    public GameObject EndingText;       // 정답 출력
    public Text warningText;            // 오답 출력
    public string PW = "0517";          // 정답 패스워드

    public void PWCheck(){
        if (inputPassword.text == PW){
            EndingText.SetActive(true);
            warningText.gameObject.SetActive(false);
            monitorObj.SetActive(false);
            GameManager.instance.gameEnd = true;
            gameObject.transform.parent.gameObject.SetActive(false);
            EndingObject.SetActive(true);
        }
        else{
            warningText.gameObject.SetActive(true);
        }
    }
}
