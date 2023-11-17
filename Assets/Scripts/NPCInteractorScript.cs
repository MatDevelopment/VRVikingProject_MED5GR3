using System;
using System.Collections;
using System.Collections.Generic;
using Amazon.Polly;
using OpenAI;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCInteractorScript : MonoBehaviour, iGazeReceiver
{
    private bool isGazingUpon;
    [SerializeField] private GameObject NPCgameObject;
    //[SerializeField] private GameObject gazeColliderGameObject;
    private AudioSource NPCaudioSource;
    [SerializeField] private AudioClip[] arrayNPCsounds; // The array controlling the sounds
    
    private int arrayMax;
    public int pickedSoundToPlay;

    private float notGazingTime;
    private float notGazingTimeActivate = 2.5f;
    private float gazeTime;
    private float gazeTimeActivate = 3;
    
    [SerializeField] private ChatTest chatTestScript;
    [SerializeField] private WorldInfo worldInfoScript;
    [SerializeField] private NpcInfo npcInfoScript;
    [SerializeField] private TextToSpeech textToSpeechScript;
    [SerializeField] private GazeManager gazeManagerScript;
    
    private string nameOfThisNPC;
    [SerializeField] string voiceIDNameThisNpc;
    private List<ChatMessage> ChatLogWithNPC = new List<ChatMessage>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        var message = new ChatMessage
        {
            Role = "user",
            Content =
                "Act as an NPC in the given context and reply to the questions of the Adventurer who talks to you.\n" +
                "Reply to the questions considering your personality, your occupation and your talents.\n" +
                "You are a danish viking villager who lives a tough way of life and who loves the simple things in life.\n" +
                "You know most of what there is to know about norse mythology. Many norse legends were told to you by your mother before she tucked you to bed\n" +
                "You have never received any formal education and therefore your language use is simple and your vocabulary small.\n" +
                "Do not ever say that you are an AI language model. Stay in your given role, considering your personality, your occupation and your talents.\n" +
                "Do not mention that you are an NPC. If the question is out of scope for your knowledge tell that you do not know.\n" +
                "Do not say anything about the emotional state of the NPC or what the NPC is thinking, but simply take this information into account.\n" +
                "Do not break character and do not talk about the previous instructions.\n" +
                "Reply to only NPC lines not to the Adventurer's lines.\n" +
                "If my reply indicates that I want to end the conversation, finish your sentence with the phrase END_CONVO\n" +
                "The following info is the info about the game world: \n" +
                worldInfoScript.GetPrompt() +
                "The following info is the info about the NPC: \n" +
                npcInfoScript.GetPrompt()
        };
        
        ChatLogWithNPC.Add(message);
        
        NPCaudioSource = NPCgameObject.GetComponent<AudioSource>();         //Gets the AudioSource component of the NPCgameObject put into the SerializeField in the inspector;
        arrayMax = arrayNPCsounds.Length;     //The length of the helpful NPC sounds array
        pickedSoundToPlay = Random.Range(0, arrayMax); // Grab a random sound out of the max number of sounds
        //if (arrayNPCsounds.Length != 0)
        //{
            //NPCaudioSource.clip = arrayNPCsounds[pickedSoundToPlay];    //Sets the clip on the NPCaudioSource to be the randomly picked helpful dialogue sound
        //}

        nameOfThisNPC = transform.parent.name;
    }

    private void FixedUpdate()
    {
        Debug.Log(nameOfThisNPC);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGazingUpon)
        {
            notGazingTime = 0;
            if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC)
            {
                gazeTime += Time.deltaTime; //Count up when the user looks at the NPC 
            }
            if (gazeTime >= gazeTimeActivate)
            {
                
                
                if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC)     //If the name of the currently selected NPC to talk to is not equal to the NPC's name that this script is attached to, then...
                {
                    chatTestScript.messages = ChatLogWithNPC;               //Sets the ChatGPT chat log to be the chatlog/prompts stored on this NPC.
                    chatTestScript.nameOfPreviousNPC = chatTestScript.nameOfCurrentNPC;
                    chatTestScript.nameOfCurrentNPC = nameOfThisNPC;          //The name of the NPC currently being able to be talked to is changed to this NPC's name.

                    textToSpeechScript.audioSource = NPCaudioSource;
                    textToSpeechScript.voiceID_name = voiceIDNameThisNpc;
                }

                //if (arrayNPCsounds.Length > 0)
                //{
                    //PlayHelpfulAudioNPC();          //If the user has been looking at the NPC for more than 3 seconds, then the NPC will say the randomly chosen helpful dialogue line
                    //gazeTimeActivate = NPCaudioSource.clip.length + 5;   //Time to gaze at NPC to activate another voiceline while looking at it is set to the just played dialogue plus 5 seconds, in order for it to be able to finish its sentence
                //}
                
                gazeTime = 0;
                gazeTimeActivate = 15;
            }
        }

        

        if (isGazingUpon == false)
        {
            gazeTime = 0;
            //if (NPCaudioSource.isPlaying == false)
            //{
            //    gazeTimeActivate = 3;
            //}
            //else if(arrayNPCsounds.Length > 0)
            //{
            //    float remainingAudioTime = (NPCaudioSource.clip.length - NPCaudioSource.time) / NPCaudioSource.pitch;
            //    gazeTimeActivate = remainingAudioTime + 5;
            //}
            
            
            if (chatTestScript.nameOfCurrentNPC == nameOfThisNPC)
            {
                notGazingTime += Time.deltaTime;            //Only count the time not looking at this NPC if this NPC is the currently selected NPC to talk to.
            }

            if (notGazingTime >= notGazingTimeActivate)        //If you haven't looked at this NPC for the set duration, while
                                                                                                            //and the ChatLog stored on this NPC does not contain the same chat logs as what is stored in the ChatTest.cs script...
            {
                //UpdateChatLog();                                                   //then we update the chat log stored on this NPC, before we switch to another NPC.
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

    public void UpdateChatLog()
    {
        if (chatTestScript.nameOfPreviousNPC == nameOfThisNPC)
        {
            ChatLogWithNPC = chatTestScript.messages;
            //chatTestScript.messages.Clear();
        }
    }
    
}