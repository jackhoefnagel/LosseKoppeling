using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextObjectScript : MonoBehaviour {

    laneScript laneScript;
    spawnObjectScript spawnObjectScript;
    GameObject currentObject;

    Rigidbody rb;
    public Text LaneScoreBoardText;
    public Text CountdownText;

    //Countdown for next
    float countdownTimer = 0f;
    float countdownDelay = 1f;
    float countdownTime = 3f;

    bool endedItem = false;

    [Header("scorevalues")]
    float minDistace;
    public float maxDistance;

    void Start()
    {
        spawnObjectScript = GetComponent<spawnObjectScript>();

        //Set the countdownTimer
        countdownTimer = countdownTime + countdownDelay;

        laneScript = GetComponent<laneScript>();
    }

    public void SetObject()
    {
        if (spawnObjectScript.lastSpawnedObject == null)
            return;

        currentObject = spawnObjectScript.lastSpawnedObject;

        rb = currentObject.GetComponent<Rigidbody>();

        //Get the minimum distance
        minDistace = Vector3.Distance(new Vector3(0, 0, 0), currentObject.transform.position);
    }

    public void ResetObject()
    {
        currentObject = null;
        rb = null;
        minDistace = 0;
    }

    void Update()
    {
        // if game is started
        if (!GameScript.instance.gameStarted || spawnObjectScript.lastSpawnedObject == null)
            return;

        if (currentObject == null)
            SetObject();

        //Calculate the score value
        float _x = Mathf.Clamp(((((maxDistance - minDistace) + (Vector3.Distance(new Vector3(0, 0, 0), currentObject.transform.position) - minDistace)) / (maxDistance - minDistace)) - 1), 0, 1);
        float _score = Mathf.Pow(_x*3,2)/3 * 100;
        LaneScoreBoardText.text = Mathf.Round(_score).ToString();

        //Check if object isnt moving
        if (pushScript.instance.selectedObject == currentObject)
        {
            if (rb.velocity.magnitude < 0.000001f && laneScript.pointNumber > 0 && laneScript.pointNumber != 4)
            {
                Countdown();
            }
            else
            {
                if(laneScript.pointNumber == 4)
                {
                    LaneScoreBoardText.text = "0";
                }
                ResetCountdown();
            }
        }
    }

    //Count down from 3
    void Countdown()
    {
        countdownTimer -= Time.deltaTime;

        if (countdownTimer <= 3f)
        {
            CountdownText.text = Mathf.Ceil(countdownTimer).ToString();

            if(countdownTimer <= 0)
            {
                pushScript.instance.NextObject();
                ResetCountdown();

                endedItem = true;

                //Add Score

                //Delete Object and spawn a new one
                spawnObjectScript.InvokeDestroyObject(1f);
            }
        }
    }

    //Reset the countdown timer
    void ResetCountdown()
    {
        countdownTimer = countdownTime + countdownDelay;
        CountdownText.text = "";
    }
}
