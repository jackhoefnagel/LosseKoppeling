using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxAngleScript : MonoBehaviour {

    public GameObject measuredObject;
    RectTransform displayTransform;
    public Text valueText;

    public bool showMaxValue;

    float maxAngle = 0;

    void Start()
    {
        displayTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        displayTransform.rotation = Quaternion.Euler(0, 0, measuredObject.transform.rotation.eulerAngles.x + 90);

        valueText.text = displayTransform.rotation.eulerAngles.z.ToString("0");

        if (!showMaxValue)
            return;

        if (maxAngle < displayTransform.rotation.eulerAngles.z)
            maxAngle = displayTransform.rotation.eulerAngles.z;

        displayTransform.rotation = Quaternion.Euler(0, 0, maxAngle);

        valueText.text = maxAngle.ToString("0");
    }
}
