using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoice : MonoBehaviour
{
    public GameObject playerObj;
    
    public void GameResume(){
        PlayerController playerCon = playerObj.GetComponent<PlayerController>();
        GameManager.instance.MouseClip(true);
        GameManager.instance.GameStop(false);
        Time.timeScale = 1;
        playerCon.IsPause = !playerCon.IsPause;
    }
    
    public void GameQuit(){
        Application.Quit();
    }
}
