using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneScript : MonoBehaviour {

    public GameObject laneObject;
    private Material laneMaterial;

    public Texture[] emissions;

    int pointNumber = 0;

    void Start()
    {
        if (laneObject != null)
        {
            laneMaterial = laneObject.GetComponent<Renderer>().material;
            //.SetTexture("_EmissionMap", emissions[3]);
        }
    }

    public void NextPoint()
    {
        pointNumber += 1;
        laneMaterial.SetTexture("_EmissionMap", emissions[pointNumber]);

        

        if(pointNumber+1 >= emissions.Length)
        {
            laneMaterial.SetColor("_Color", Color.gray);

            GameScript.instance.Score -= 3;
        } else
        {
            GameScript.instance.Score++;
        }
    }
}
