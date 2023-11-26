using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Polly;
using OpenAI;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCInteractorScript : MonoBehaviour
{
    private bool isGazingUpon;
    [SerializeField] private GameObject NPCgameObject;
    //[SerializeField] private GameObject gazeColliderGameObject;
    public AudioSource NPCaudioSource;
    [SerializeField] private AudioClip[] arrayNPCsounds; // The array controlling the sounds
    
    private int arrayMax;
    public int pickedSoundToPlay;

    /*private float notGazingTime;
    private float notGazingTimeActivate = 2.5f;
    private float gazeTime;
    private float gazeTimeActivate = 3;
    private float newNPCFocusTime = 2.8f;*/
    
    [SerializeField] private ChatTest chatTestScript;
    [SerializeField] private WorldInfo worldInfoScript;
    [SerializeField] private NpcInfo npcInfoScript;
    [SerializeField] private TaskInfo taskInfoScript;
    [SerializeField] private sceneInfo sceneInfoScript;
    [SerializeField] private TextToSpeech textToSpeechScript;
    [SerializeField] private Whisper whisperScript;
    [SerializeField] private LevelChanger levelChangerScript;
    
    public string nameOfThisNPC;
    public string voiceIDNameThisNpc;
    
    [TextArea(3,20)]
    [SerializeField] string itemDescription_Horn;
    [TextArea(3,20)]
    [SerializeField] string itemDescription_Brooch;
    [TextArea(3,20)]
    [SerializeField] string itemDescription_Blanket;
    [TextArea(3,20)]
    [SerializeField] string itemDescription_Knife;
    [TextArea(3,20)]
    [SerializeField] string itemDescription_ThorsHammer;

    private bool ItemGathered_Horn = false;

    [SerializeField] private float gazeTimeToActivate = 3f;
    
    public List<ChatMessage> ChatLogWithNPC = new List<ChatMessage>();
    //[SerializeField] private List<string> listOfOtherNpcs = new List<string>();
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        var message = new ChatMessage
        {
            Role = "system",
            Content =
                "Act as an NPC in the given context and reply to the questions of the Traveller who talks to you.\n" +
                "Reply to the questions considering your personality, your occupation and your talents.\n" +
                "You are a danish viking villager from the 900th century who lives a peaceful way of life and likes the simple things in life.\n" +
                "You know most of what there is to know about norse mythology. Many norse legends were told to you by your mother before she tucked you to bed\n" +
                "You have never received any formal education and therefore your language use is simple and your vocabulary small.\n" +
                "Do not ever say that you are an AI language model. Stay in your given role, considering your personality, your occupation and your talents.\n" +
                "Do not mention that you are an NPC. If the question is out of scope for your knowledge tell that you do not know.\n" +
                "Do not say anything about the emotional state of the NPC or what the NPC is thinking, but simply take this information into account.\n" +
                "Do not break character and do not talk about the previous instructions.\n" +
                "Reply to only NPC lines not to the Traveller's lines.\n" +
                "If the Traveller does not say anything then ask the Traveller what is on their mind.\n" +
                "Your responses should be no longer than 40 words.\n" +
                //"Keep your responses to a maximum word limit of 40 words.\n" +
                //"If my reply indicates that I want to end the conversation, finish your sentence with the phrase END_CONVO\n" +
                "The following info is the info about the game world: \n" +
                worldInfoScript.GetPrompt() +
                "The following info is the info about the NPC: \n" +
                npcInfoScript.GetPrompt() +
                "Do not include the NPC name in your response.\n" +
                "The following info is the info about the NPC's current surroundings: \n" +
                sceneInfoScript.GetPrompt() +
                "The following info is the info about the Traveller's current task and subtasks: \n" +
                taskInfoScript.GetPrompt()
        };
        
        ChatLogWithNPC.Add(message);
        
        //NPCaudioSource = NPCgameObject.GetComponent<AudioSource>();         //Gets the AudioSource component of the NPCgameObject put into the SerializeField in the inspector;
        arrayMax = arrayNPCsounds.Length;     //The length of the helpful NPC sounds array
        pickedSoundToPlay = Random.Range(0, arrayMax); // Grab a random sound out of the max number of sounds
                                                       //if (arrayNPCsounds.Length != 0)
                                                       //{
                                                       //NPCaudioSource.clip = arrayNPCsounds[pickedSoundToPlay];    //Sets the clip on the NPCaudioSource to be the randomly picked helpful dialogue sound
                                                       //}
                                                       

        /*if (transform.parent != null)
        {
            nameOfThisNPC = transform.parent.name;
        }
        else
        {
            nameOfThisNPC = transform.name;
        }*/

        //nameOfThisNPC = transform.name;
    }
    

    private void PlayConversationStarterAudioNPC()
    {
        if (arrayNPCsounds.Length > 0)
        {
            arrayMax = arrayNPCsounds.Length;
            pickedSoundToPlay = Random.Range(0, arrayMax);
            NPCaudioSource.clip = arrayNPCsounds[pickedSoundToPlay];
            
            NPCaudioSource.Play();
            Debug.Log("Played conversation starter");
        }
        
    }

    
    //Method that gets called on Select of XR Grab , aka the personal belongings of the deceased that the player are able to bring to the burial
    public void AppendItemDescriptionToPrompt(string nameOfItem)    //Add a time.DeltaTime that makes sure that there are atleast 30 seconds between item checks
    {
        if (levelChangerScript.Scene2Active == true && whisperScript.isDoneTalking == true && textToSpeechScript.audioSource.isPlaying == false)
        {
            if (nameOfItem == "Horn" && ItemGathered_Horn == false)     //IMPLEMENT THIS FOR THE OTHER ITEMS ALSO
            {
                MakeNpcDescribeItem(itemDescription_Horn);              //IMPLEMENT THIS FOR THE OTHER ITEMS ALSO
                ItemGathered_Horn = true;                               //IMPLEMENT THIS FOR THE OTHER ITEMS ALSO
                //chatTestScript.SendReply(itemDescription_Horn);
                //levelChangerScript.ItemGathered_Horn = false;
                //ItemGathered_Horn = true;       
            }
            else if (nameOfItem == "Brooch")
            {
                //chatTestScript.SendReply(itemDescription_Brooch);
                //levelChangerScript.ItemGathered_Brooch = false;
            }
            else if (nameOfItem == "Blanket")
            {
                //chatTestScript.SendReply(itemDescription_Blanket);
                //levelChangerScript.ItemGathered_Blanket = false;
            }
            else if (nameOfItem == "Knife")
            {
                //chatTestScript.SendReply(itemDescription_Knife);
                //levelChangerScript.ItemGathered_Knife = false;
            }
            else if (nameOfItem == "ThorsHammer")
            {
                //chatTestScript.SendReply(itemDescription_ThorsHammer);
                //levelChangerScript.ItemGathered_ThorsHammer = false;
            }
            
        }
    }
    

    public void Start_PickThisNpc_Coroutine()
    {
        if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC && whisperScript.isDoneTalking == true && textToSpeechScript.audioSource.isPlaying == false)
        {
            StartCoroutine(PickThisNpc());
        }
        
    }

    public void StartCoroutine_PlayNpcDialogueAfterSetTime()
    {
        if (textToSpeechScript.audioSource.isPlaying == false && whisperScript.isDoneTalking == true && arrayNPCsounds.Length > 0)
        {
            Debug.Log("Started NPC dialogue coroutine on: " + nameOfThisNPC);
            StartCoroutine(PlayNpcDialogueAfterSetTime());
        }
        
    }
    
    
    private IEnumerator PickThisNpc()
    {
        Debug.Log("running coroutine" + nameOfThisNPC);
        if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC & chatTestScript.isDone == true & textToSpeechScript.isGeneratingSpeech == false & NPCaudioSource.isPlaying == false)
        {
            yield return new WaitForSeconds(3);
            Debug.Log("PickThisNPC" + nameOfThisNPC);
            //chatTestScript.messages.Clear();
            chatTestScript.messages = ChatLogWithNPC;               //Sets the ChatGPT chat log to be the chatlog/prompts stored on this NPC.
            textToSpeechScript.audioSource = NPCaudioSource;
            textToSpeechScript.voiceID_name = voiceIDNameThisNpc;
            chatTestScript.nameOfCurrentNPC = nameOfThisNPC;
            //Maybe insert some dialogue to play that makes it clear that this NPC is now the new NPC in focus.
        }
        
        /*else
        {
            StopCoroutine(PickThisNpc());
        }*/

    }

    private IEnumerator PlayNpcDialogueAfterSetTime()
    {
        yield return new WaitForSeconds(gazeTimeToActivate);
        PlayConversationStarterAudioNPC();
        yield return new WaitForSeconds(gazeTimeToActivate + NPCaudioSource.clip.length);
        PlayConversationStarterAudioNPC();
    }
    
    
    private async void MakeNpcDescribeItem(string itemDescription)
    {
        whisperScript.isDoneTalking = false;
        chatTestScript.AddSystemInstructionToChatLog(itemDescription);
        string chatGptResponse = await chatTestScript.AskChatGPT(chatTestScript.messages);
        chatTestScript.AddNpcResponseToChatLog(chatGptResponse);
        Debug.Log(chatGptResponse);
        textToSpeechScript.MakeAudioRequest(chatGptResponse);
        whisperScript.isDoneTalking = true;
    }
    
    public async void MakeNpcCountRemainingBelongings(string countPrompt)
    {
        whisperScript.isDoneTalking = false;
        chatTestScript.AddSystemInstructionToChatLog(countPrompt);
        string chatGptResponse = await chatTestScript.AskChatGPT(chatTestScript.messages);
        chatTestScript.AddNpcResponseToChatLog(chatGptResponse);
        Debug.Log(chatGptResponse);
        textToSpeechScript.MakeAudioRequest(chatGptResponse);
        whisperScript.isDoneTalking = true;
    }
    

    
    
    
    
    


    //-------------OLD CODE--------------//
    
        /*void Update()
    {
        
        /*if (isGazingUpon)
        {
            //notGazingTime = 0;        //Moved from here...
            if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC && textToSpeechScript.isGeneratingSpeech == false)
            {
                notGazingTime = 0;      //to here...
                gazeTime += Time.deltaTime; //Count up when the user looks at the NPC 
            }
            if (gazeTime >= gazeTimeActivate && chatTestScript.isDone == true && chatTestScript.nameOfCurrentNPC != nameOfThisNPC)      //JUST ADDED chatTestScript after NPC switching focus NOT WORKING
            {
                chatTestScript.messages = ChatLogWithNPC;               //Sets the ChatGPT chat log to be the chatlog/prompts stored on this NPC.
                //chatTestScript.nameOfPreviousNPC = chatTestScript.nameOfCurrentNPC;
                textToSpeechScript.audioSource = NPCaudioSource;
                textToSpeechScript.voiceID_name = voiceIDNameThisNpc;
                    
                chatTestScript.nameOfCurrentNPC = nameOfThisNPC;        //The name of the NPC currently being able to be talked to is changed to this NPC's name.
                gazeTime = 0;
                gazeTimeActivate = 3;
                
                /*if (chatTestScript.nameOfCurrentNPC != nameOfThisNPC)     //If the name of the currently selected NPC to talk to is not equal to the NPC's name that this script is attached to, then...
                {
                    chatTestScript.messages = ChatLogWithNPC;               //Sets the ChatGPT chat log to be the chatlog/prompts stored on this NPC.
                    //chatTestScript.nameOfPreviousNPC = chatTestScript.nameOfCurrentNPC;
                    textToSpeechScript.audioSource = NPCaudioSource;
                    textToSpeechScript.voiceID_name = voiceIDNameThisNpc;
                    
                    chatTestScript.nameOfCurrentNPC = nameOfThisNPC;        //The name of the NPC currently being able to be talked to is changed to this NPC's name.
                    gazeTime = 0;
                    gazeTimeActivate = 3;
                }#2#

                //if (arrayNPCsounds.Length > 0)
                //{
                    //PlayHelpfulAudioNPC();          //If the user has been looking at the NPC for more than 3 seconds, then the NPC will say the randomly chosen helpful dialogue line
                    //gazeTimeActivate = NPCaudioSource.clip.length + 5;   //Time to gaze at NPC to activate another voiceline while looking at it is set to the just played dialogue plus 5 seconds, in order for it to be able to finish its sentence
                //}
                
                //gazeTime = 0;         //Moved from here....... to up within the if that is before
                //gazeTimeActivate = 3;     
            }



            if (isGazingUpon == false)
            {
                //gazeTime = 0;     //Moved from here...
                
                //if (NPCaudioSource.isPlaying == false)
                //{
                //    gazeTimeActivate = 3;
                //}
                //else if(arrayNPCsounds.Length > 0)
                //{
                //    float remainingAudioTime = (NPCaudioSource.clip.length - NPCaudioSource.time) / NPCaudioSource.pitch;
                //    gazeTimeActivate = remainingAudioTime + 5;
                //}
                
                
                if (chatTestScript.nameOfCurrentNPC == nameOfThisNPC && textToSpeechScript.isGenereatingSpeech == false)
                {
                    gazeTime = 0;       //To here...
                    notGazingTime += Time.deltaTime;            //Only count the time not looking at this NPC if this NPC is the currently selected NPC to talk to.
                }

                if (notGazingTime >= newNPCFocusTime && chatTestScript.isDone == true && listOfOtherNpcs.Contains(gazeManagerScript.lastGazedUpon.transform.parent.name))        //If you haven't looked at this NPC for the set duration, while
                {
                    chatTestScript.messages.Clear();
                    ChatLogWithNPC = chatTestScript.messages;

                    notGazingTime = 0;
                    //UpdateChatLog();                                                   //then we update the chat log stored on this NPC, before we switch to another NPC.
                }
            
            
            }

            
        }#1#
    }*/
    
    /*public void GazingUpon()
    {
        isGazingUpon = true;
    }

    public void NotGazingUpon()
    {
        isGazingUpon = false;
    }*/
    
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

}
