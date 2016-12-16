using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraPositionScript : MonoBehaviour {

    public SixenseHands side;
    public SixenseInput.Controller m_controller = null;

    float m_fLastTriggerVal;
    Vector3 m_initialPosition;
    Quaternion m_initialRotation;

    protected void Start()
    {
        // get the Animator
        m_initialRotation = transform.localRotation;
        m_initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (m_controller == null)
        {
            m_controller = SixenseInput.GetController(side);
        }
    }

    public Quaternion InitialRotation
    {
        get { return m_initialRotation; }
    }

    public Vector3 InitialPosition
    {
        get { return m_initialPosition; }
    }
}
