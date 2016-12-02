using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject testObject;

    void Start()
    {
        SpawnTestObject();
    }

    void Update()
    {
        //spawn a test object when the control key is hit
        if (Input.GetKeyDown(KeyCode.LeftControl))
            SpawnTestObject();
    }

    //Spawn a testobject in front of the player
    void SpawnTestObject()
    {
        Instantiate(testObject, new Vector3(0, 15, 0), Quaternion.Euler(88.4203f, -35.0701f, -83.2231f));
    }
}
