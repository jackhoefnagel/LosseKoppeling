using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneScript : MonoBehaviour {

    spawnObjectScript spawnObjectScript;
    public GameObject laneObject;
    private Material laneMaterial;

    public Texture[] emissions;

    public int pointNumber = 0;

    void Start()
    {
        spawnObjectScript =  GetComponent<spawnObjectScript>();

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

            pushScript.instance.InvokeNextObject(1f);
            //Delete Object and spawn a new one
            spawnObjectScript.InvokeDestroyObject(1f);
        } else
        {
            GameScript.instance.Score++;
        }
    }

    public void ResetLane()
    {
        pointNumber = 0;
        laneMaterial.SetColor("_Color", Color.white);
        laneMaterial.SetTexture("_EmissionMap", emissions[0]);
    }
}
