using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCInteractorScript : MonoBehaviour, iGazeReceiver
{
    private bool isGazingUpon;
    [SerializeField] private GameObject NPCgameObject;
    //[SerializeField] private GameObject gazeColliderGameObject;
    private AudioSource NPCaudioSource;
    public AudioClip[] arrayNPCsounds; // The array controlling the sounds
    
    private int arrayMax;
    public int pickedSoundToPlay;

    private float gazeTime;
    private float gazeTimeActivate = 3;
    
    
    // Start is called before the first frame update
    void Start()
    {
        NPCaudioSource = NPCgameObject.GetComponent<AudioSource>();         //Gets the AudioSource component of the NPCgameObject put into the SerializeField in the inspector;
        arrayMax = arrayNPCsounds.Length;     //The length of the helpful NPC sounds array
        pickedSoundToPlay = Random.Range(0, arrayMax); // Grab a random sound out of the max number of sounds
        NPCaudioSource.clip = arrayNPCsounds[pickedSoundToPlay];    //Sets the clip on the NPCaudioSource to be the randomly picked helpful dialogue sound
    }

    // Update is called once per frame
    void Update()
    {
        if (isGazingUpon)
        {
            gazeTime += Time.deltaTime;        //When the user looks at the NPC 
            if (gazeTime >= gazeTimeActivate)
            {
                PlayHelpfulAudioNPC();          //If the user has been looking at the NPC for more than 3 seconds, then the NPC will say the randomly chosen helpful dialogue line
                gazeTime = 0;
                gazeTimeActivate = NPCaudioSource.clip.length + 5;     //Time to gaze at NPC to activate another voiceline while looking at it is set to the just played dialogue plus 4 seconds, in order for it to be able to finish its sentence
            }
        }

        if (isGazingUpon == false)
        {
            gazeTime = 0;
            if (NPCaudioSource.isPlaying == false)
            {
                gazeTimeActivate = 3;
            }
            else
            {
                float remainingAudioTime = (NPCaudioSource.clip.length - NPCaudioSource.time) / NPCaudioSource.pitch;
                gazeTimeActivate = remainingAudioTime + 5;
            }
        }
    }
    
    public void GazingUpon()
    {
        isGazingUpon = true;
    }

    public void NotGazingUpon()
    {
        isGazingUpon = false;
    }
    
    /*private void OnCollisionEnter(Collision other)
    {
        collisionTime = 0;      //The time that the user have looked at the NPC is set to 0, effectively being reset
    }

    private void OnCollisionStay(Collision other)
    {
        collisionTime += Time.deltaTime;        //When the user looks at the NPC 
        if (collisionTime >= 3)
        {
            PlayHelpfulAudioNPC();          //If the user has been looking at the NPC for more than 3 seconds, then the NPC will say the randomly chosen helpful dialogue line
        }
    }*/
    

    public void PlayHelpfulAudioNPC()
    {
        arrayMax = arrayNPCsounds.Length;
        pickedSoundToPlay = Random.Range(0, arrayMax);
        NPCaudioSource.clip = arrayNPCsounds[pickedSoundToPlay];
        
        NPCaudioSource.Play();
    }
    
}
