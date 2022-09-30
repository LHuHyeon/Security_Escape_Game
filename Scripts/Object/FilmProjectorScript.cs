using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilmProjectorScript : MonoBehaviour
{
    public GameObject missionObj;

    // private void OnTriggerStay(Collider other) {
    //     if (other.tag == "Player" && Input.GetMouseButtonDown(1)){
    //         missionObj.SetActive(true);
    //     }
    // }
    public void LastMission(){
        missionObj.SetActive(true);
        gameObject.SetActive(false);
    }
}
