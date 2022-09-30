using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public float lockNumber;         // 자물쇠 번호와 키 번호와 같은지 확인할 변수

    public GameObject doorLock;     // 잠긴 문을 오픈할 문

    public bool KeyCheck(float keyNumber){
        if (lockNumber == keyNumber){
            DoorController doorCon = doorLock.GetComponent<DoorController>();
            doorCon.doorLock = false;
            doorCon.doorEvent();

            gameObject.SetActive(false);

            return true;
        }
        return false;
    }
}
