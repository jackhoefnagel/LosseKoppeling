using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class BoxingGloveImpactScript : MonoBehaviour {
    
    public ParticleSystem punchImpactParticles;
    public ParticleSystem punchImpactParticles2;
    AudioSource hitSound;

    VirtualHandScript AScript;
    float Strength = 0;
    float Speed = 0;

    public bool useRageMeter = true;
    public bool useVisualFeedback = true;
    VignetteAndChromaticAberration effect;
    bool hit = false;

    [Range(0.1f, 1)]
    float visualfeedbackMultiplier = 0.1f;

    void Awake() {
        punchImpactParticles.Stop();

        effect = Camera.main.GetComponent<VignetteAndChromaticAberration>();
    }

    void Start()
    {
        AScript = GetComponent<VirtualHandScript>();
        hitSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateStrengthSpeed();

        if (!useVisualFeedback)
            return;

        if(true)
        {
            float _totalImpact2 = ((Strength * 3) * Mathf.Abs(Speed));

            if (float.IsNaN(_totalImpact2))
                return;

            effect.intensity = Mathf.Clamp(Mathf.Lerp(effect.intensity, (_totalImpact2 / 2.3f) * visualfeedbackMultiplier, Time.deltaTime * 3), 0.13f, 0.4f);
            effect.chromaticAberration = Mathf.Lerp(effect.chromaticAberration, (_totalImpact2 * 25) * visualfeedbackMultiplier, Time.deltaTime * 3);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            PlayParticle();
        }
    }

    public void PlayParticle()
    {
        float _totalImpact = (((Strength * 3) * Mathf.Abs(Speed)) / 2 + 0.4f) * visualfeedbackMultiplier;
        float _totalImpact2 = (((Strength * 3) * Mathf.Abs(Speed)) + 0.3f) * visualfeedbackMultiplier;
        _totalImpact2 = Mathf.Clamp(_totalImpact2, 0.3f, 1.2f);

        Debug.Log(_totalImpact);
        if(useRageMeter)
            rageScript.instance.AddScore(_totalImpact);

        punchImpactParticles.startSize = _totalImpact * .5f;

        punchImpactParticles2.startSize = _totalImpact2 * 0.07f;
        punchImpactParticles2.startLifetime = _totalImpact2 * 0.3f;
        punchImpactParticles2.startSpeed = _totalImpact2 * 4f;

        hitSound.volume = _totalImpact2;
        hitSound.Play();

        //AScript.myoDataScript.Vibrate(Thalmic.Myo.VibrationType.Short);

        punchImpactParticles.Play();
    }

    void UpdateStrengthSpeed()
    {
        Strength = AScript.GetStrength();
        Speed = AScript.GetSpeed();
    }
}
