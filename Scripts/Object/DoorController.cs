using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    AudioSource audioSource;            // 오디오 소스
    public AudioClip audioDoorOpen;     // 열리는 소리
    public AudioClip audioDoorClose;    // 닫히는 소리
    public AudioClip audioDoorLock;     // 잠긴 소리

    public GameObject itemObject;          // 안에 있는 아이템

    bool doorMove = false;          // 문 움직임 확인

    public float rotSpeed = 2f;     // 속도
    public float doorMax = 90f;     // 열리는 회전각 (y)
    public float doorMin = 0f;      // 닫히는 회전각 (y)

    public bool hasDoor = false;    // 문 열림 여부 변수
    public bool doorLock;           // 문 잠금 변수
    public bool hasItem = false;    // 안에 아이템이 있는가?

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasItem){   // 아이템을 보관하는 문인가?
            Item itemObj = itemObject.GetComponent<Item>();
            hasItem = itemObj.hasDoor;
        }
        if (hasDoor){   // 문 열기
            Quaternion targetRotation = Quaternion.Euler(0, doorMax, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotSpeed*Time.deltaTime);
            audioSource.clip = audioDoorOpen;
            if (hasItem){
                itemObject.SetActive(true);
            }
        }
        if (!hasDoor){  // 문 닫기
            Quaternion targetRotation2 = Quaternion.Euler(0, doorMin, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, rotSpeed*Time.deltaTime);
            audioSource.clip = audioDoorClose;
            if (hasItem){
                itemObject.SetActive(false);
            }
        }
        if (doorMove){  // 문 여는 소리
            if (!audioSource.isPlaying){
                audioSource.Play();
            }
            doorMove = false;
        }
    }

    public void doorEvent(){
        if (!doorLock){ // 열림 상태일 때
            // 문을 클릭 시 대화창이 출력되지 않도록 isDoor = true를 해줍니다.
            if (gameObject.GetComponent<ObjectData>() != null)
                gameObject.GetComponent<ObjectData>().isDoor = true;

            hasDoor = !hasDoor; // 문 작동
            doorMove = true;
            GameManager.instance.hasDoorLock = false;
        }
        else{   // 잠김 상태일 때
            GameManager.instance.hasDoorLock = true;
            if (gameObject.GetComponent<ObjectData>() != null)
                gameObject.GetComponent<ObjectData>().isDoor = false;
            
            audioSource.Stop();     // 오디오 끄기
            audioSource.clip = audioDoorLock;
            audioSource.Play();     // 오디오 실행
        }
    }
}
