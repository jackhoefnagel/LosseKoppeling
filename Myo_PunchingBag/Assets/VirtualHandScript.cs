using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualHandScript : MonoBehaviour {

    public ThalmicMyo myo;
    ThalmicMyo myoDataScript;

    float strength = 0;
    float speed = 0;
    float x = 0;

    public GameObject Object;
    Rigidbody rb;

    public float force;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myoDataScript = myo.GetComponent<ThalmicMyo>();
    }


    void Update()
    {
        if (Object == null)
            return;

        transform.rotation = Object.transform.rotation;

        rb.velocity = (Object.transform.position - transform.position) * force;

        //Add force
        strength = CalculatePower();
        speed = rb.velocity.magnitude;
        
        float _mass = Mathf.Clamp(strength * strength * strength * 1, 0.001f,5);
        rb.mass = _mass;
    }
    
    //Calculate the amount of force the user applies
    float CalculatePower()
    {
        float totalEMGValue = 0;

        foreach (var sensor in myoDataScript.emg)
        {
            totalEMGValue += Mathf.Abs(sensor);
        }

        totalEMGValue /= myoDataScript.emg.Length;

        totalEMGValue = Mathf.Clamp((totalEMGValue - 10) / 35, 0f, 10f);

        //Set particle emission
        /*
        power.emissionRate = particleEmission * totalEMGValue;
        if (useVisualFeedback)
        {
            effect.intensity = Mathf.Lerp(effect.intensity, (totalEMGValue / 2.3f), Time.deltaTime * 3);
            effect.chromaticAberration = Mathf.Lerp(effect.chromaticAberration, (totalEMGValue * 25), Time.deltaTime * 3);
        }*/

        return totalEMGValue;
    }

    //Return strength
    public float GetStrength() { return strength; }
    //Return speed
    public float GetSpeed() { return speed; }
}
