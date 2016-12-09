using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObjectScript : MonoBehaviour {

    public GameObject lastSpawnedObject;
    GameObject lastSpawnedObjectCollider;
    nextObjectScript nextObjectScript;
    laneScript laneScript;

    public GameObject[] spawnableObjects;
    [Range(0, 4)]
    public int id;

    int[] chances = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 5 };

    void Start()
    {
        nextObjectScript = GetComponent<nextObjectScript>();
        laneScript = GetComponent<laneScript>();

        lastSpawnedObject = Instantiate(spawnableObjects[id], transform.position, transform.rotation);
        nextObjectScript.maxDistance = lastSpawnedObject.GetComponent<sizeScript>().maxDistance;
        lastSpawnedObjectCollider = lastSpawnedObject.transform.Find("collider").gameObject;
        lastSpawnedObjectCollider.GetComponent<colliderScript>().parentObject = transform.gameObject;

        nextObjectScript.SetObject();
    }

    void SpawnNewObject()
    {
        int _randomNumber = chances[Random.Range(0, chances.Length)];

        lastSpawnedObject = Instantiate(spawnableObjects[_randomNumber-1], transform.position, transform.rotation);
        nextObjectScript.maxDistance = lastSpawnedObject.GetComponent<sizeScript>().maxDistance;
        lastSpawnedObjectCollider = lastSpawnedObject.transform.Find("collider").gameObject;
        lastSpawnedObjectCollider.GetComponent<colliderScript>().parentObject = transform.gameObject;

        nextObjectScript.SetObject();
    }

    public void InvokeDestroyObject(float time)
    {
        Invoke("DestroyObject", time);
    }

    void DestroyObject()
    {
        DestroyObject(lastSpawnedObject);
        lastSpawnedObject = null;

        nextObjectScript.ResetObject();
        laneScript.ResetLane();

        Invoke("SpawnNewObject",1f);
    }
}
