using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundScript : MonoBehaviour {

    float speed = 0;
    AudioSource movingSound;
    Rigidbody rb;

    void Start()
    {
        movingSound = transform.GetComponent<AudioSource>();
        rb = transform.GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        speed = rb.velocity.magnitude;

        movingSound.volume = 0.15f * speed * (rb.mass/10);
    }
}
