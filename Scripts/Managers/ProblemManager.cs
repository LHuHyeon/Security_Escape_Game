using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    public GameObject ProblemUI;
    public bool hasTalk;

    // Player가 문제 오브젝트를 터치할 시 문제 UI 출력
    public void Mission(){
        if (hasTalk){   // 대화창 출력
            GameManager.instance.Action(gameObject);
            hasTalk = false;
            return;
        }
        GameManager.instance.ProblemUI(ProblemUI);
    }
}
