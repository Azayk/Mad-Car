using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;

    public float loopDuration = 3.5f; // Duration of the loop in seconds
    public float startOffset = 0.5f; // Start sound from 0.5 seconds

    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();

        carAudio.time = startOffset; // Set start time to 0.5 seconds
        carAudio.Play(); // Start playing the sound
    }

    void Update()
    {
        EngineSound();

        if (carAudio.time >= loopDuration)
        {
            carAudio.time = startOffset; // Reset audio time to 0.5 seconds
        }
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchFromCar = carRb.velocity.magnitude / 60f;

        if (currentSpeed < minSpeed)
        {
            carAudio.pitch = minPitch;
        }
        else if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch + pitchFromCar;
        }
        else if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }
}