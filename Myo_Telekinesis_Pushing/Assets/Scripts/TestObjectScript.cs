using UnityEngine;
using System.Collections;

public class TestObjectScript : MonoBehaviour {

	void Update () {
        //Destroy object when below field
        if (transform.position.y < -5)
            Destroy(transform.gameObject);
	}
}
