using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;


public class CollisionSound : MonoBehaviour
{
    AudioSource audioSource;

    private int arrayMax;
    public int soundToPlay;

    [SerializeField] private AudioClip[] arrayPhysicSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    void OnCollisionEnter(Collision collision) 
    {
        arrayMax = arrayPhysicSounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arrayPhysicSounds[soundToPlay];

        audioSource.volume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 15);
       
        if (collision.relativeVelocity.magnitude > 2)
            audioSource.Play();
    }

}