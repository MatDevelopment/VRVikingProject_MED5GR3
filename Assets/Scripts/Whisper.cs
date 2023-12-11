using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
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
        [SerializeField] private Image progress;
        [SerializeField] private InputActionReference buttonHoldReference = null;
        
        private readonly string fileName = "output.wav";
        private readonly int duration = 10;
        
        private AudioClip clip;
        
        public bool isRecording = false;
        public bool isDoneTalking = true;
        
        private float time;
        private float timeToInterruptTalk = 0.25f;
        
        private OpenAIApi openai = new OpenAIApi();

        private void Awake()
        {
            if (LevelChanger.LLM_VersionPlaying == true)
            {
                buttonHoldReference.action.Enable();
                buttonHoldReference.action.performed += StartRecording;
                buttonHoldReference.action.canceled += StartRecording;
            }
        }

        private void OnDestroy()
        {
            if (LevelChanger.LLM_VersionPlaying == false)
            {
                buttonHoldReference.action.started -= StartRecording;
                buttonHoldReference.action.performed -= StartRecording;
                buttonHoldReference.action.canceled -= StartRecording;
            }
            
        }

        private void Start()
        {
            
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
            if (context.canceled && LevelChanger.LLM_VersionPlaying == true)
            {
                //time = 0;
                isRecording = false;
                progress.fillAmount = 1;
                Debug.Log("Stop recording...");
                    
                Microphone.End(null);
                byte[] data = SaveWav.Save(fileName, clip);
            
                var req = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() {Data = data, Name = "audio.wav"},
                    // File = Application.persistentDataPath + "/" + fileName,
                    Prompt = "The transcript is the dialogue of a danish villager from the 900th century. This danish villager has a danish accent when speaking english.",
                    Model = "whisper-1",
                    Language = "en"
                };
                var res = await openai.CreateAudioTranscription(req);
                
                Debug.Log("Recording: " + res.Text);

                if (res.Text.Length >= 38)          //If what the user says is longer than 38 characters (including spaces), then the current NPC will say a thinking sound like "Hmm" or "Hmm, let me think"
                {
                    StartCoroutine(SayThinkingSoundAfterPlayerTalked());
                }

                chatTest.AddPlayerInputToChatLog(res.Text);
                isDoneTalking = false;
                // Debug.Log($"isDoneTalking: {isDoneTalking}");
                string chatGptResponse = await chatTest.AskChatGPT(chatTest.messages);
                chatTest.AddNpcResponseToChatLog(chatGptResponse);
                Debug.Log(chatGptResponse);
                textToSpeechScript.MakeAudioRequest(chatGptResponse);
                isDoneTalking = true;
                // Debug.Log($"isDoneTalking: {isDoneTalking}");

            }
            if(context.performed && LevelChanger.LLM_VersionPlaying == true)
            {
                StartCoroutine(InterruptNpcTalkingAfterDuration(timeToInterruptTalk));
                Debug.Log("Start recording...");
                isRecording = true;
    
                var index = PlayerPrefs.GetInt("user-mic-device-index");
                clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
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
        }
        
        
        public IEnumerator SayThinkingSoundAfterPlayerTalked()      //Gets called in Whisper.cs after the user stops talking (context.cancelled)
        {
            yield return new WaitForSeconds(0.5f);
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
        
    }
}
