using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad_Control : MonoBehaviour
{
    public int buttonIndex;
    public GameObject Keypad;
    KeyPad Problem;

    void Start() {
        Problem = Keypad.GetComponent<KeyPad>();
    }

    public void ButtonClick(){
        Problem.KeyCheck(buttonIndex);
    }

    public void GreenButton(){
        Problem.AnswerCheck();
    }

    public void RedButton(){
        Problem.KeyMin();
    }
}
