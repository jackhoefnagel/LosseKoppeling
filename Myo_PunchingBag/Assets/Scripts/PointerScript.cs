using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PointerScript : MonoBehaviour {

    public float maxGrabDistance = 50;
    ForceCalculator forceScript;
    GroundModifier vectorScript;

    public GameObject debugCube;
    public GameObject accelerationCube;

    private Vector3 velocity = Vector3.zero;

    [Range(0, 10000)]
    public float grabForce = 5000;

    [Range(0, 100)]
    public float speedMultiplier = 100;

    float forwardSpeed = 0;
    bool isThrowing = false;

    //Object
    public GameObject grabbedObject;
    Rigidbody grabbedObjectRB;
    public float objectDistance2D;

    public GameObject projectorObj;
    Projector projector;
    bool grabbedFloor = false;
    Vector3 floorPoint = Vector3.zero;

    void Start()
    {
        forceScript = GetComponent<ForceCalculator>();
        vectorScript = GetComponent<GroundModifier>();
        projector = projectorObj.GetComponent<Projector>();
    }

	void Update()
    {
        //Reset Game
        if (Input.GetKeyDown(KeyCode.R))
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        //Draw a debug raycast
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawLine(transform.position, fwd.normalized * maxGrabDistance, Color.red);

        //Check for movable objects
        RaycastHit hit;
        if(Physics.Raycast(transform.position, fwd.normalized, out hit, maxGrabDistance))
        {
            if(hit.transform.gameObject.CompareTag("movable") && grabbedObject == null && isThrowing == false && !grabbedFloor)
            {
                grabbedObject = hit.transform.gameObject;
                grabbedObjectRB = grabbedObject.GetComponent<Rigidbody>();

                objectDistance2D = Vector3.Distance(new Vector3(grabbedObject.transform.position.x, 0, grabbedObject.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
            }

            if (hit.transform.gameObject.CompareTag("ground") && grabbedObject == null && !grabbedFloor)
            {
                projector.enabled = true;
                projectorObj.transform.position = new Vector3(hit.point.x, projectorObj.transform.position.y, hit.point.z);
                floorPoint = new Vector3(hit.point.x, projectorObj.transform.position.y, hit.point.z);
            } else
            {
                if(!grabbedFloor)
                    projector.enabled = false;
            }
        } else
        {
            if(!grabbedFloor)
                projector.enabled = false;
        }

        //Calculate acceleration
        float _acceleration = forceScript.forwardAcceleration - fwd.y;

        //Add the acceleration
        if ((_acceleration > 0.02f || _acceleration < -0.02f) || (forwardSpeed > 0.1f || forwardSpeed < -0.1f))
            forwardSpeed = Mathf.Lerp(forwardSpeed, (forwardSpeed + _acceleration), 3 * Time.deltaTime);
        else
            if ((forwardSpeed < 0.2f || forwardSpeed > -0.2f) && forwardSpeed != 0)
                forwardSpeed = 0;

        //Reset forwardspeed
        if(fwd.y > 0.6 || fwd.y < -0.5)
        {
            forwardSpeed = 0;
        }

        //Debug acceleration to cube
        accelerationCube.transform.position = new Vector3(forwardSpeed, .5f, -9);

        //If object is grabbed move it
        if (grabbedObject != null)
        {
            //Calculate where the object should move to
            Vector3 moveTowardsXZ = new Vector3(fwd.x, 0, fwd.z).normalized * objectDistance2D;
            float moveTowardY = Mathf.Tan(fwd.y) * objectDistance2D;

            //Move grabbed object
            grabbedObjectRB.AddForce((transform.position + new Vector3(moveTowardsXZ.x, moveTowardY, moveTowardsXZ.z) - grabbedObject.transform.position)/1.3f * forceScript.combinedStrength * grabForce * Time.deltaTime);
            grabbedObjectRB.AddTorque((transform.position + new Vector3(moveTowardsXZ.x, moveTowardY, moveTowardsXZ.z) - grabbedObject.transform.position)/5 * forceScript.combinedStrength, ForceMode.Force);
            debugCube.transform.position = transform.position + new Vector3(moveTowardsXZ.x, moveTowardY, moveTowardsXZ.z);

            //If forward speed is bigger that value release object
            if (forwardSpeed > 0.4f || forwardSpeed < -0.35f)
                ThrowGrabbedObject(fwd,forwardSpeed);

            //Reset the grabbed object if the distance is too far
            if (Vector3.Distance(transform.position + new Vector3(moveTowardsXZ.x, moveTowardY, moveTowardsXZ.z), grabbedObject.transform.position) > 8)
                ResetGrabbedObject();

            //Reset grabbed object when force is too low
            if (forceScript.combinedStrength < 0.25f)
                ResetGrabbedObject();
        }

        grabbedFloor = false;

        if (grabbedObject == null && projector.enabled)
        {
            if (forceScript.combinedStrength > 0.33f)
            {
                grabbedFloor = true;


                vectorScript.point = floorPoint;
                vectorScript.range = 1.5f;
                float _distance = Vector3.Distance(new Vector3(floorPoint.x, 0, floorPoint.z), new Vector3(transform.position.x, 0, transform.position.z));
                vectorScript.height = Mathf.Tan(fwd.y) * _distance;
                vectorScript.strength = forceScript.combinedStrength;

                vectorScript.moveGround = true;
            } else
            {
                vectorScript.moveGround = false;
            }
        }
        else
        {
            vectorScript.moveGround = false;
        }    
    }

    void ThrowGrabbedObject(Vector3 throwDirection, float throwForce)
    {
        isThrowing = true;

        if(throwForce > 0)
            grabbedObjectRB.AddForce(throwDirection * throwForce * forceScript.combinedStrength * (grabForce/2.3f));
        else
        {
            grabbedObjectRB.AddForce(throwDirection * throwForce * forceScript.combinedStrength * (grabForce / 5f));
        }

        Invoke("ResetGrabbedObject", 10 * Time.deltaTime);
    }

    void ResetGrabbedObject()
    {
        grabbedObject = null;
        grabbedObjectRB = null;

        isThrowing = false;
        forwardSpeed = 0;
        
        objectDistance2D = 0;

        debugCube.SetActive(false);
    }
}
