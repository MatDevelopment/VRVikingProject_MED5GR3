using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
        //[SerializeField] private Button recordButton;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private ChatTest chatTest;
        [SerializeField] private TextToSpeech textToSpeechScript;
        //[SerializeField] private LevelChanger levelChangerScript;
        [SerializeField] private LLMversionPlaying LLMversionPlayingScript;
        [SerializeField] private Image progress;
        [SerializeField] private InputActionReference buttonHoldReference = null;

        private readonly string deleteThisObligatedStringFromWhisperCreditString = "Subs by www.zeoranger.co.uk";
        private readonly string fileName = "output.wav";
        private readonly int duration = 12;
        
        private AudioClip clip;
        [FormerlySerializedAs("hmmThinkingSound")] [SerializeField] private AudioClip ErikHmmSound;
        [SerializeField] private AudioClip ArneHmmSound;
        [SerializeField] private AudioClip FridaHmmSound;
        [SerializeField] private AudioClip IngridHmmSound;
        
        public bool isRecording = false;
        public bool isDoneTalking = true;
        public bool contextIsPerformed = false;
        
        private float time;
        private float timeToInterruptTalk = 0.15f;
        
        private OpenAIApi openai = new OpenAIApi();

        private void Awake()
        {
            LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
        }

        private void OnDestroy()
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                buttonHoldReference.action.Disable();
                buttonHoldReference.action.started -= StartRecording;
                buttonHoldReference.action.performed -= StartRecording;
                buttonHoldReference.action.canceled -= StartRecording;
            }
            
        }

        private void Start()
        {
            //levelChangerScript = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
            
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                buttonHoldReference.action.Enable();
                buttonHoldReference.action.performed += StartRecording;
                buttonHoldReference.action.canceled += StartRecording;
            }

            foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            //recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        private async void StartRecording(InputAction.CallbackContext context)
        {
            if (context.canceled && LLMversionPlayingScript.LLMversionIsPlaying == true && contextIsPerformed == true && isRecording == true)
            {
                //time = 0;
                progress.fillAmount = 1;
                Debug.Log("Stop recording...");
                    
                Microphone.End(null);
                
                byte[] data = SaveWav.Save(fileName, clip);
            
                var req = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() {Data = data, Name = "audio.wav"},
                    // File = Application.persistentDataPath + "/" + fileName,
                    Prompt = "The transcript is the dialogue of a person with a danish accent.",
                    Model = "whisper-1",
                    Language = "en"
                };
                var res = await openai.CreateAudioTranscription(req);
                
                /*if ((res.Text.Contains("Hello") && res.Text.Length < 11) || (res.Text.Contains("Hi") && res.Text.Length < 11))      //If the user's input to the NPC is Hello or Hi, and what they say is less than 11 characters long, including spaces, 
                {                                                                                       //then the NPC only says "Hmm" and not "Let me think", which is AN audio clip contained within the sound clip array with
                    StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource));         //thinking sounds. Let me think would be an unusual response to someone saying hello to you.
                }*/
                
                if(res.Text.Contains("Hello") || res.Text.Contains("Hi"))
                {
                    StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource, 0.2f));
                }
                else if ((res.Text.Length >= 12 && !res.Text.Contains("Hello")) || (res.Text.Length >= 12 && !res.Text.Contains("Hi")))          //If what the user says is longer than 12 characters (including spaces), then the current NPC will say a thinking sound like "Hmm" or "Hmm, let me think" or "Hmm let me think for a second".
                {
                    StartCoroutine(SayThinkingSoundAfterPlayerTalked());
                }
                else if (res.Text.Contains(deleteThisObligatedStringFromWhisperCreditString))
                {
                    res.Text = res.Text.Replace(deleteThisObligatedStringFromWhisperCreditString, "");
                }
                else if (res.Text == deleteThisObligatedStringFromWhisperCreditString)
                {
                    res.Text = "";
                }
                isRecording = false;
                
                if (string.IsNullOrEmpty(res.Text) == false || string.IsNullOrWhiteSpace(res.Text) == false)
                {
                    Debug.Log("Recording: " + res.Text);
                    
                    chatTest.AddPlayerInputToChatLog(res.Text);
                    isDoneTalking = false;
                    // Debug.Log($"isDoneTalking: {isDoneTalking}");
                    string chatGptResponse = await chatTest.SendRequestToChatGpt(chatTest.messages);
                    chatTest.AddNpcResponseToChatLog(chatGptResponse);
                    Debug.Log(chatGptResponse);
                    textToSpeechScript.MakeAudioRequest(chatGptResponse);
                    isDoneTalking = true;
                    // Debug.Log($"isDoneTalking: {isDoneTalking}");
                    res.Text = res.Text.Replace(res.Text, "");
                }
                
                contextIsPerformed = false;
            }
            if(context.performed && LLMversionPlayingScript.LLMversionIsPlaying == true && isRecording == false && contextIsPerformed == false && isDoneTalking == true)
            {
                StartCoroutine(InterruptNpcTalkingAfterDuration(timeToInterruptTalk));
                Debug.Log("Start recording...");
                isRecording = true;
    
                var index = PlayerPrefs.GetInt("user-mic-device-index");
                clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
                contextIsPerformed = true;
            }
        }
        
        /*private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progress.fillAmount = time / duration;      //Meant for showing how much time you have left to talk in through a fill amount of a UI progress bar etc. Not being used currently.
            }
            
            if(time >= duration)
            {
                time = 0;
                progress.fillAmount = 0;        //Meant for showing how much time you have left to talk in through a fill amount of a UI progress bar etc. Not being used currently.
            }
        }*/

        private IEnumerator InterruptNpcTalkingAfterDuration(float interruptDuration)
        {
            yield return new WaitForSeconds(interruptDuration);
            textToSpeechScript.audioSource.Stop();
            StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource, 0.1f));
        }
        
        
        public IEnumerator SayThinkingSoundAfterPlayerTalked()      //Gets called in Whisper.cs after the user stops talking (context.cancelled)
        {
            yield return new WaitForSeconds(0.2f);
            PickThinkingSoundToPlay(textToSpeechScript.audioSource);
        }
    
        private void PickThinkingSoundToPlay(AudioSource audioSourceToPlayOn)
        {
            if (chatTest.currentNpcThinkingSoundsArray.Length > 0)
            {
                int arrayThinkingSoundsMax = chatTest.currentNpcThinkingSoundsArray.Length;
                int pickedThinkingSoundToPlay = Random.Range(0, arrayThinkingSoundsMax);
                audioSourceToPlayOn.clip = chatTest.currentNpcThinkingSoundsArray[pickedThinkingSoundToPlay];
                
                audioSourceToPlayOn.Play();
            }
        }

        private IEnumerator PlayHmmThinkingSound(AudioSource audioSourceToPlayOn, float timeTillPlaySound)
        {
            yield return new WaitForSeconds(timeTillPlaySound);
            switch (chatTest.nameOfCurrentNPC)
            {
                case "Erik":
                    audioSourceToPlayOn.clip = ErikHmmSound;
                    break;
                case "Frida":
                    audioSourceToPlayOn.clip = FridaHmmSound;
                    break;
                case "Arne":
                    audioSourceToPlayOn.clip = ArneHmmSound;
                    break;
                case "Ingrid":
                    audioSourceToPlayOn.clip = IngridHmmSound;
                    break;
                default:
                    audioSourceToPlayOn.clip = ErikHmmSound;
                    break;
            }
            
            audioSourceToPlayOn.Play();
        }
    }
}
