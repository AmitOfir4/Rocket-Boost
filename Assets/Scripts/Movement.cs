using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftEngineParticles;
    [SerializeField] ParticleSystem rightEngineParticles;
    AudioSource audioSource;
    Rigidbody rb;
    float rotationInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            startThrust();
        }
        else
        {
            stopThrust();
        }
    }
    private void startThrust()
    {
        PlaySound();
        PlayParticles(mainEngineParticles);
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
    }

    private void stopThrust()
    {
        StopSound();
        stopParticles(mainEngineParticles);
    }

    private void ProcessRotation()
    {
        rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0) // rotate left
        {
            RotateLeft();
        }
        else if (rotationInput > 0)
        {
            RotateRight();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationStrength);
        stopParticles(rightEngineParticles);
        PlayParticles(leftEngineParticles);
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationStrength);
        stopParticles(leftEngineParticles);
        PlayParticles(rightEngineParticles);
    }

    private void ApplyRotation(float rotationStrength)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationStrength * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    private void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSound);
        }
    }

    private void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayParticles(ParticleSystem particles)
    {
        if (particles != null && !particles.isPlaying)
        {
            particles.Play();
        }
    }
    
    private void stopParticles(ParticleSystem particles)
    {
        if (particles != null && particles.isPlaying)
        {
            particles.Stop();
        }
    }
}
