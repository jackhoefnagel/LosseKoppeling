using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ForceCalculator : MonoBehaviour {

    public GameObject MyoData = null;
    private ThalmicMyo myoDataScript;

    public Text maxValueLabel;
    private Text[] perSensorMaxValueLabel;

    private float[] previousData = { 0, 0, 0, 0, 0, 0, 0, 0 };
    private Vector3 previousAcceleration = Vector3.zero;

    //Select mode
    private bool previousPerSensorValue;
    public bool perSensorValue = false;

    public bool debugMode = false;

    //min and max values per sensor
    private float[] minData = { 10, 10, 10, 10, 10, 10, 10, 10 };
    private float[] maxData = { 0, 0, 0, 0, 0, 0, 0, 0 };

    //min and max values of all sensors
    private float minValue = 10;
    private float maxValue = 0;

    public float combinedStrength;
    public float realStrength;
    public float maxStregnth;
    public float forwardAcceleration;

    [Header("display bars")]
    public GameObject[] displayBars;
    public GameObject strengthBar;
    public ParticleSystem forceParticles;
    public Canvas debugCanvas;

    void Start()
    {
        //Set Debugmode
        if (debugMode)
        {
            debugCanvas.enabled = debugMode;
        }

        //Assign the myo script
        myoDataScript = MyoData.GetComponent<ThalmicMyo>();

        //Assign all the Text components
        perSensorMaxValueLabel = new Text[displayBars.Length];

        for (int i = 0; i < displayBars.Length; i++)
        {
            perSensorMaxValueLabel[i] = displayBars[i].transform.Find("maxValueLabel").GetComponent<Text>();
        }

        //Set the per sensor option
        previousPerSensorValue = perSensorValue;
        SetPerSensorDisplayMode();

        //VibrateMyo();
    }

    void Update()
    {

        ////////////////accelorometer sensor////////////////
        forwardAcceleration = myoDataScript.accelerometer.z;

        ////////////////EMG sensor////////////////

        //Check if the data exists
        if (myoDataScript.emg == null)
            return;

        //Check for Calibrate button
        if (Input.GetKeyDown(KeyCode.C))
            ResetCalibration();

        //Check if displaymode is changed
        if(previousPerSensorValue != perSensorValue)
        {
            previousPerSensorValue = perSensorValue;
            SetPerSensorDisplayMode();
        }

        //Combined Strength Variable
        float _combinedPercentage = 0;

        //calculate the force and visualize the bars
        for (int i = 0; i < myoDataScript.emg.Length; i++)
        {
            previousData[i] = Mathf.Lerp(previousData[i], Mathf.Abs(myoDataScript.emg[i]), 3 * Time.deltaTime);

            float _sensorValue = previousData[i];

            //Set the minimum value per sensor
            if (minData[i] > _sensorValue)
                minData[i] = _sensorValue;

            //Set the maximum value per sensor
            if (maxData[i] < _sensorValue)
            {
                maxData[i] = _sensorValue;
                perSensorMaxValueLabel[i].text = ((int)maxData[i]).ToString();
            }

            //Set maximul value of all sensors
            if (maxValue < _sensorValue)
            {
                maxValue = _sensorValue;
                maxValueLabel.text = ((int)maxValue).ToString();
            }

            //Set minimum value of all sensors
            if (minValue > _sensorValue)
                minValue = _sensorValue;

            //Initialize the variables
            float _difference;
            float _newValue;

            float _percentage;

            //Per sensor
            if (perSensorValue) {
                //Calculate the percentage
                _difference = maxData[i] - minData[i];
                _newValue = _sensorValue - minData[i];

                _percentage = (float)_newValue / (float)_difference;

                //Visualize the bars
                displayBars[i].GetComponent<Image>().fillAmount = _percentage;
            } else
            {
                //Calculate the percentage
                _difference = maxValue - minValue;
                _newValue = _sensorValue - minValue;

                _percentage = (float)_newValue / (float)_difference;

                //Visualize the bars
                displayBars[i].GetComponent<Image>().fillAmount = _percentage;
            }

            _combinedPercentage += _percentage;
        }

        //Calculate the combined Strength
        combinedStrength = _combinedPercentage / myoDataScript.emg.Length;


        //Strenght visuals
        strengthBar.GetComponent<Image>().fillAmount = combinedStrength;
        forceParticles.emissionRate = combinedStrength * 110 - 25;
    }

    //Reset the calibrated data
    void ResetCalibration()
    {
        //Reset all min and max values
        float[] _newMinData = { 10, 10, 10, 10, 10, 10, 10, 10 };
        float[] _newMaxData = { 0, 0, 0, 0, 0, 0, 0, 0 };

        minData = _newMinData;
        maxData = _newMaxData;

        minValue = 10;
        maxValue = 0;

        //Reset the labels
        maxValue = 0;
        for (int i = 0; i < maxData.Length; i++)
        {
            maxData[i] = 0;
        }

        minValue = 10;
        for (int i = 0; i < minData.Length; i++)
        {
            minData[i] = 10;
        }
    }

    //Change display mode (per sensor/ all sensors)
    void SetPerSensorDisplayMode()
    {
        maxValueLabel.enabled = !perSensorValue;

        foreach (var label in perSensorMaxValueLabel)
        {
            label.enabled = perSensorValue;
        }
    }

    void VibrateMyo()
    {
        myoDataScript.NotifyUserAction();

        Invoke("VibrateMyo", Time.deltaTime*7);
    }
}
