using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSrc;
    public bool hasStop;

    void Start() {
        audioSrc = GetComponent<AudioSource>();
    }

    // 플레이어와 부딪치면 실행
    private void OnTriggerEnter(Collider other) {
        if (hasStop){
            audioSrc.Play();
            hasStop = false;
            Debug.Log("음악 실행.");
        }
    }
}
