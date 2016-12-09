using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class pushScript : MonoBehaviour {

    public static pushScript instance = null;

    void Awake()
    {
        instance = this;
    }

    //Myo
    public GameObject MyoData = null;
    private ThalmicMyo myoDataScript;

    //Force
    [Range(0, 1000)]
    public float maxStrength = 500f;
    float Force = 0;

    //Feedback
    public ParticleSystem power;
    float particleEmission = 0;

    public bool useVisualFeedback = true;
    VignetteAndChromaticAberration effect;

    //Object
    public GameObject selectedObject;
    private Rigidbody rb;

    //Rotation
    [Header("Movement")]
    [Range(1, 10)]
    public int amountOfObjects;
    float currentRotation = 0;
    public bool useArmRotationForMovement;

    [Range(0, 500)]
    public int nextValue;
    [Range(0,10)]
    public float movementCooldown;

    bool isTurning = false;
    float movementCooldownTimer;


    void Start()
    {
        //Get the myo script
        myoDataScript = MyoData.GetComponent<ThalmicMyo>();

        //Set the timer
        movementCooldownTimer = movementCooldown;

        //Get particle emission
        particleEmission = power.emissionRate;
        power.emissionRate = 0;

        effect = Camera.main.GetComponent<VignetteAndChromaticAberration>();
    }

    void Update()
    {
        //If the game is started
        if (!GameScript.instance.gameStarted || GameScript.instance.gameEnded)
            return;

        CheckNewObject();

        Force = Mathf.Lerp(Force,CalculatePower(),3*Time.deltaTime);

        if (selectedObject != null && !float.IsNaN(Force))
            AddForceToObject(Force);

        if(moveCooldown())
            checkMovement();

        MovePlayer();
        

        //Debug the speed
        //Debug.Log((transform.position .z- 3.3f) / Time.time);
    }

    //Change the current gameobject
    void SetNewObject(GameObject newObject = null)
    {
        selectedObject = newObject;

        if (selectedObject != null)
            rb = selectedObject.GetComponent<Rigidbody>();
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

        totalEMGValue = Mathf.Clamp((totalEMGValue - 10) / 35,0f,10f);

        //Set particle emission
        power.emissionRate = particleEmission * totalEMGValue;
        if (useVisualFeedback)
        {
            effect.intensity = Mathf.Lerp(effect.intensity, (totalEMGValue / 2.3f), Time.deltaTime * 3);
            effect.chromaticAberration = Mathf.Lerp(effect.chromaticAberration, (totalEMGValue * 25), Time.deltaTime * 3);
        }

        return totalEMGValue;
    }

    //Add force to object
    void AddForceToObject(float ForceAmount)
    {
        rb.AddForce(selectedObject.transform.forward * maxStrength * ForceAmount, ForceMode.Force);
    }

    //Check if the player should move
    void checkMovement()
    {
        //Use arm for moving
        if (useArmRotationForMovement)
        {
            if (myoDataScript.gyroscope.z > nextValue && !isTurning)
            {
                NextObject();
            }

            if (myoDataScript.gyroscope.z < -nextValue && !isTurning)
            {
                PreviousObject();
            }
        }
        //Use arrows for moving
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isTurning)
            {
                NextObject();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isTurning)
            {
                PreviousObject();
            }
        }
    }

    public void InvokeNextObject(float time)
    {
        Invoke("NextObject", time);
    }

    //Turn player to the next object
    public void NextObject()
    {
        Debug.Log("Next");
        isTurning = true;
        currentRotation += (360 / amountOfObjects);
    }

    //Turn player to the previous object
    public void PreviousObject()
    {
        Debug.Log("Previous");
        isTurning = true;
        currentRotation -= (360 / amountOfObjects);
    }

    //Move the player
    void MovePlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, currentRotation, transform.rotation.z), Time.deltaTime * 4);
    }

    //Can the player move again
    bool moveCooldown()
    {
        if (!isTurning)
            return true;

        if(movementCooldownTimer <= 0)
        {
            isTurning = false;
            movementCooldownTimer = movementCooldown;
            return true;
        }

        //Twice as fast for arrow mode
        if (!useArmRotationForMovement)
            movementCooldownTimer -= Time.deltaTime;

        movementCooldownTimer -= Time.deltaTime;

        return false;
    }

    //CheckNewObject
    void CheckNewObject()
    {
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(transform.position.x, 1, transform.position.z), transform.forward);
        Debug.DrawLine(new Vector3(transform.position.x, 1, transform.position.z), new Vector3(transform.position.x, 1, transform.position.z) + transform.forward * 12, Color.red);


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "movable")
            {
                SetNewObject(hit.transform.gameObject);
            }
        }
    }
}
