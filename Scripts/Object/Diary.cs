using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    public GameObject diaryUI;

    public void DiaryOpen(){
        GameManager.instance.PlayerStop(true);
        diaryUI.SetActive(true);
        GameManager.instance.teamObject = diaryUI.gameObject;
        GameManager.instance.hasObject = true;
    }
}
