using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderScript : MonoBehaviour {

    public GameObject parentObject;
    laneScript laneScript;

    void Start()
    {
        if(parentObject == null)
            parentObject = transform.parent.gameObject;

        laneScript = parentObject.GetComponent<laneScript>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("PointCollider"))
        {
            laneScript.NextPoint();
        }
    }
}
