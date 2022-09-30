using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speak : MonoBehaviour
{
    public GameObject Speak1;

    public void StartSpeak()
    {
        GameManager.instance.Action(Speak1);
    }
}
