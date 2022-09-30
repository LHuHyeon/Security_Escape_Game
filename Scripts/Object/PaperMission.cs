using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperMission : MonoBehaviour
{
    public GameObject missionObject;

    public void CallObject(){
        GameManager.instance.ProblemUI(missionObject);
        GameManager.instance.hasPaper = true;
    }
}
