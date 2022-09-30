using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MissionButton : MonoBehaviour
{
    public void EndButton(){
        gameObject.transform.parent.gameObject.SetActive(false);
        GameManager.instance.PlayerStop(false);
    }
    
    public void FileOpen(){
        Process.Start(Application.dataPath + "/exeDerectory");
    }

    public void FileExecution(){
        Process.Start(Application.dataPath + "/exeDerectory/Q1.exe");
    }

    public void CMDExecution(){
        Process.Start("cmd.exe");
    }
}
