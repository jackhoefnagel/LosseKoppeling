using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rageScript : MonoBehaviour {

    public static rageScript instance;

    void Awake()
    {
        instance = this;
    }

    float visualScore = 0;
    float score = 0;

    Image image;
    AudioSource audio;

    public float decreaseAmount = 1f;

    void Start()
    {
        image = GetComponent<Image>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        score -= (decreaseAmount + score*0.01f) * Time.deltaTime;
        score = Mathf.Clamp(score, 0, 100);

        visualScore = Mathf.Lerp(visualScore, score, Time.deltaTime * 1.5f);

        image.fillAmount = visualScore / 100;
        audio.volume = (visualScore / 100) * (visualScore / 100);
    }

    public void AddScore(float power)
    {
        power *= 5;

        score += power;
        score = Mathf.Clamp(score, 0, 100);
    }
}
