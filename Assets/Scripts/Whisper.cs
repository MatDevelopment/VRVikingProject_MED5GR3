using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
        //[SerializeField] private Button recordButton;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private ChatTest chatTest;
        [SerializeField] private Image progress;
        [SerializeField] private InputActionReference buttonHoldReference = null;
        
        private readonly string fileName = "output.wav";
        private readonly int duration = 10;
        
        private AudioClip clip;
        private bool isRecording = false;
        private float time;
        private OpenAIApi openai = new OpenAIApi();

        private void Awake()
        {
            buttonHoldReference.action.Enable();
            buttonHoldReference.action.performed += StartRecording;
            buttonHoldReference.action.canceled += StartRecording;
        }

        private void OnDestroy()
        {
            buttonHoldReference.action.started -= StartRecording;
            buttonHoldReference.action.performed += StartRecording;
            buttonHoldReference.action.canceled += StartRecording;
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
            if (context.canceled)
            {          
                time = 0;
                isRecording = false;
                progress.fillAmount = 1;
                Debug.Log("Stop recording...");
                    
                Microphone.End(null);
                byte[] data = SaveWav.Save(fileName, clip);
            
                var req = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() {Data = data, Name = "audio.wav"},
                    // File = Application.persistentDataPath + "/" + fileName,
                    Model = "whisper-1",
                    Language = "en"
                };
                var res = await openai.CreateAudioTranscription(req);

                chatTest.SendReply(res.Text);
            }
            if(context.performed)
            {
                Debug.Log("Start recording...");
                isRecording = true;
    
                var index = PlayerPrefs.GetInt("user-mic-device-index");
                clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
            }
        }
        
        private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progress.fillAmount = time / duration;
            }
            
            if(time >= duration)
            {
                time = 0;
                progress.fillAmount = 0;
            }
        }
    }
}
