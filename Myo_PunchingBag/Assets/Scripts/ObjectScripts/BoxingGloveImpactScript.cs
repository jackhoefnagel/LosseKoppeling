using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingGloveImpactScript : MonoBehaviour {

    public ParticleSystem punchImpactParticles;

    void Awake(){
        punchImpactParticles.Stop();
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Object")){
            //if velocity magnitude > ?
            punchImpactParticles.Play();
        }
    }
}
