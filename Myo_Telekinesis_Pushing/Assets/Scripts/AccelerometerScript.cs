﻿using UnityEngine;
using System.Collections.Generic;

public class AccelerometerScript : MonoBehaviour {

    //Assign the myo object
    [Header("Myo")]
    public GameObject MyoData = null;
    private ThalmicMyo myoDataScript;

    //Acceleration Data
    Vector3 lastA = Vector3.zero;
    Vector3 lastV = Vector3.zero;
    float[] lastStrength = { 0, 0, 0, 0, 0, 0, 0, 0 };

    //Punch Data
    bool disablePunch = false;
    bool punch = false;
    List<float> punchSpeeds;
    List<float> punchStrenghts;


    void Start()
    {
        //Assign the myo script
        myoDataScript = MyoData.GetComponent<ThalmicMyo>();

        //Reset the data
        ResetPunchData();
    }

    void Update()
    {
        float _velocity = CalculateAcceleration();
        float _strength = CalculateStrength();

        transform.position += Vector3.forward * _velocity;

        CheckPunch(_velocity, _strength);
    }

    /// <summary>
    /// Calculate the current velocity
    /// </summary>
    /// <returns></returns>
    float CalculateAcceleration()
    {
        Vector3 _a = myoDataScript.accelerometer;
        Vector3 _g = myoDataScript.gyroscope;

        Vector3 _v = lastA - _a;

        float _velocity = (lastV.z + _v.z) / 2;

        lastV = _v;
        lastA = _a;

        return _velocity;
    }

    float CalculateStrength()
    {
        float _combinedSensorValues = 0;

        for (int i = 0; i < myoDataScript.emg.Length; i++)
        {
            lastStrength[i] = Mathf.Lerp(lastStrength[i], Mathf.Abs(myoDataScript.emg[i]), 3 * Time.deltaTime);

            _combinedSensorValues += lastStrength[i];
        }

        float _maxStrengthPercentage = _combinedSensorValues / (128 * myoDataScript.emg.Length);

        return _maxStrengthPercentage;
    }

    /// <summary>
    /// Checks if the player punches
    /// </summary>
    /// <param name="velocity"></param>
    void CheckPunch(float velocity, float strength)
    {
        if (velocity > 0.5 || (punch && velocity > 0f))
        {
            punch = true;

            punchSpeeds.Add(velocity);
            punchStrenghts.Add(strength);
        }
        else
        {
            if (punch)
            {
                Debug.LogWarning("Punch");
                Debug.Log("Speed: " + Mathf.Max(punchSpeeds.ToArray()));
                Debug.Log("Strength: " + Mathf.Max(punchStrenghts.ToArray()));

                ResetPunchData();
            }
        }
    }

    /// <summary>
    /// Reset the punch data after the player has punched
    /// </summary>
    void ResetPunchData()
    {
        punch = false;
        punchSpeeds = new List<float>();
        punchStrenghts = new List<float>();
    }
}
