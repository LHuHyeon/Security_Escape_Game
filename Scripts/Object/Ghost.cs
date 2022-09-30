using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    Renderer renderer;
    public GameObject ghostObject;
    public bool hasStop;
    float value=255;
    

    void Start() {
        renderer = ghostObject.GetComponent<Renderer>();
    }

    private void GhostEvent(){
        GameManager.instance.Action(ghostObject);
        Color ghostColor = renderer.material.color;

        while(true){
            value -= (Time.deltaTime * (float)0.5);
            if (value <= 0) break;
            ghostColor.a = value;
            renderer.material.color = ghostColor;
        }

        if (value <= 0){
            // gameObject.SetActive(false);
            ghostObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && hasStop){
            GhostEvent();
            hasStop = false;
        }
    }
}
