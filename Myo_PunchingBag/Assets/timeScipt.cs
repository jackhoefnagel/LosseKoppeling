using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeScipt : MonoBehaviour {

    float timer = 0;
    Text timerText;

    void Start()
    {
        timerText = GetComponent<Text>();
    }

	void Update () {
        timer = Time.time;

        displayTime(timer);
	}

    void displayTime(float time)
    {
        timerText.text = (Mathf.Floor(time / 60)).ToString("0") + ":" + (Mathf.Ceil(time % 60)).ToString("00");
    }
}
