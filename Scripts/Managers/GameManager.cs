using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤
    public GameObject playerObject;
    public PlayerController playerObj;

    public GameObject staminaObject;

    public bool gameEnd = false;

    public bool hasPaper = false;     // 종이 미션 여부

//  [ 대화 관련 변수 ]  ---------------------
    public TalkManager talkManager;
    public int talkIndex;
    public bool talkAction=true;
    public Text talkText;               // 출력시킬 문자열
    public GameObject talkObject;
    public GameObject scanObject;       // 스캔된 오브젝트

//  [ 문 관련 변수 ]  ---------------------
    public bool hasDoorLock = true;

//  [ 문제 관련 변수 ]  ---------------------
    public GameObject teamObject;         // 호출된 문제

//  [ 정지 관련 변수 ]  ---------------------
    public GameObject EscObj;
    public bool hasObject = false;

//  [ 세이브 관련 변수 ] --------------------
    public GameObject SaveObj;

//  [ 아이템 관련 변수 ] --------------------
    public Image pick_ItemImage;
    public Text pick_ItemName;
    public Image[] itemSlotImage;
//  ----------------------------------------

//  [ 타이머 관련 변수 ] --------------------
    public Text timeText;
    public bool minhasTime = true;
    int minTime = 0;
    string timeStr = null;
    float _timeCnt = 0f;
    int hours = 0;
    int min = 0;
    public int second = 0;
//  ----------------------------------------

    void Awake() {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // 씬에 두개 이상의 GameManager 오브젝트가 존재한다는 의미.
            // 싱글톤 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
        playerObj = playerObject.GetComponent<PlayerController>();
    }

    // 해당 오브젝트에 대한 대화창을 출력
    public void Action(GameObject scanObj){
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();

        if (objData != null){
            if (!objData.isDoor){
                talkObject.SetActive(true);
                PlayerStop(true);
                MouseClip(true);
                Talk(objData.id);
            }
        }
    }

    public void Action(){
        talkObject.SetActive(true);
        PlayerStop(true);
        MouseClip(true);
        Talk(1);
        Debug.Log("Action()");
    }

    // 대화창 출력
    void Talk(int id){
        string talkData = talkManager.GetTalk(id, talkIndex);
        if (talkData == null){
            talkObject.SetActive(false);
            PlayerStop(false);
            talkAction = false;
            talkIndex = 0;
            return;
        }
        
        talkText.text = talkData;
        
        talkAction = true;
        talkIndex++; 
        Debug.Log(talkIndex);
    }

    // esc, space 누를 시 사용자 움직임 and 대화창 등이 종료됨.
    public void AnyKey(){
        if (hasObject){
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)){
                PlayerStop(false);
                if (teamObject != null) teamObject.SetActive(false);
                if (talkAction){
                    Action(scanObject);
                }
            }
        }
        if (hasPaper){
            if (Input.GetMouseButtonDown(0)){
                PlayerStop(false);
                if (teamObject != null) teamObject.SetActive(false);
                hasPaper = false;
            }
        }
    }

    // 마우스 움직임
    public void MouseClip(bool hasMouse){
        if (hasMouse){
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!hasMouse){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void GameStop(bool hasGame){
        EscObj.SetActive(hasGame);
    }

    public void Save(bool hasSave){
        if (hasSave == true){
            SaveObj.SetActive(hasSave);
            GameStop(false);
        }
        else{
            SaveObj.SetActive(hasSave);
        }
    }

    // 문제UI 출력
    public void ProblemUI(GameObject ProblemObj){
        teamObject = ProblemObj;
        ProblemObj.SetActive(true);
        PlayerStop(true);
    }

    // 스테미나 UI
    public void Stamina(float gauge){
        Image stamina = staminaObject.GetComponent<Image>();
        stamina.fillAmount = gauge;
    }

    // 플레이어를 정지시킨다.
    public void PlayerStop(bool has){
        if (has){
            hasObject = true;
            playerObj.hasStop = false;
            MouseClip(false);
        }
        else{
            hasObject = false;
            playerObj.hasStop = true;
            MouseClip(true);
        }
    }

    // 타이머
    public void TimeUI(){
        if (minhasTime) minTime = (int)Time.time;

        second = (int)(Time.time - _timeCnt) - minTime;
                        
        if( second > 59 ){
            _timeCnt = Time.time;
            second = 0;
            min++;
            
            if( min > 59 ){
                min = 0;
                hours++;
            }
        }
        
        timeStr = string.Format("{0:00} : {1:00} : {2:00}", hours, min, second);
        timeText.text = timeStr;
    }

    // 아이템 선택
    public void PickItem(string ItemName, int SlotValue){
        pick_ItemImage.sprite = itemSlotImage[SlotValue].sprite;
        pick_ItemName.text = ItemName;
    }
    public void PickItem(int SlotValue){
        pick_ItemImage.sprite = null;
        pick_ItemName.text = "빈 슬롯 " + (int)(SlotValue+1);
    }

    // 아이템 흭득
    public void ItemAdd(Sprite ItemImage, int SlotValue){
        itemSlotImage[SlotValue].sprite = ItemImage;
    }

    // 들고있는 아이템 버리기
    public void ItemRemove(){
        pick_ItemImage.sprite = null;
        pick_ItemName.text = null;
    }
}