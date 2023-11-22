using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace OpenAI
{
    public class ChatTest : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        public string nameOfPreviousNPC;
        public string nameOfCurrentNPC;
        [SerializeField] private NpcInfo npcInfo;
        [SerializeField] private WorldInfo worldInfo;
        [SerializeField] private GazeManager gazeManager;

        [SerializeField] private NPCInteractorScript erikInteractorScript;
        [SerializeField] private NPCInteractorScript arneInteractorScript;
        [SerializeField] private NPCInteractorScript fridaInteractorScript;
        
        //[SerializeField] private NpcDialog npcDialog;
        
        [SerializeField] private TextToSpeech textToSpeech;
        
        public UnityEvent OnReplyReceived;
        
        private string response;        //This string is what ChatGPT answers after a request
        public bool isDone = true;
        private RectTransform messageRect;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        public List<ChatMessage> messages = new List<ChatMessage>();

        private void Start()
        {
            //nameOfPreviousNPC = nameOfCurrentNPC;
            button.onClick.AddListener(SendReply);
            if (nameOfCurrentNPC == "Erik")
            {
                textToSpeech.audioSource = erikInteractorScript.NPCaudioSource;
                //Debug.Log("erik");
            }
            if (nameOfCurrentNPC == "Arne")
            {
                textToSpeech.audioSource = arneInteractorScript.NPCaudioSource;
            }
            if (nameOfCurrentNPC == "Frida")
            {
                textToSpeech.audioSource = fridaInteractorScript.NPCaudioSource;
            }
            
            
        }
        
        private RectTransform AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            
            if (message.Role != "user")
            {
                messageRect = item;
            }
            
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);

            if (message.Role == "user")
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                height += item.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;
            }

            return item;
        }

        private void SendReply()
        {
            SendReply(inputField.text);
        }

        public void SendReply(string input)
        {
            var message = new ChatMessage()
            {
                Role = "user",
                Content = input
            };
            messages.Add(message);
            if (nameOfCurrentNPC == "Erik")
            {
                erikInteractorScript.ChatLogWithNPC.Add(message);
            }
            if (nameOfCurrentNPC == "Arne")
            {
                arneInteractorScript.ChatLogWithNPC.Add(message);
            }
            if (nameOfCurrentNPC == "Frida")
            {
                fridaInteractorScript.ChatLogWithNPC.Add(message);
            }
            

            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-16k-0613",
                Messages = messages
            }, OnResponse, OnComplete, new CancellationTokenSource());
            
            AppendMessage(message);
            
            inputField.text = "";
        }

        private void OnResponse(List<CreateChatCompletionResponse> responses)
        {
            var text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));

            if (text == "") return;

            /*if (text.Contains("END_CONVO"))           //An example of how you could make sure the prompting (chatlog)
            {                                           with the OpenAI API gets wiped if the ChatGPT feels like you wanna
                text = text.Replace("END_CONVO", "");   end the conversation
                
                Invoke(nameof(EndConvo), 5);
            }*/
            
            if (text.Contains("FINISH_SEN"))            //This gets appended to the sentence when ChatGPT finishes its response
            {                                           
                text = text.Replace("FINISH_SEN", "");      //so here we make sure to remove it from the text so it isn't added to the AWS polly audiorequest
                
            }
            
            var message = new ChatMessage()
            {
                Role = "assistant",
                Content = text
            };

            if (isDone)
            {
                OnReplyReceived.Invoke();
                messageRect = AppendMessage(message);
                isDone = false;
                //Debug.Log(text);
            }
            
            messageRect.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            
            response = text;
            Debug.Log(response);
        }
        
        private void OnComplete()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
            height += messageRect.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            
            var message = new ChatMessage()
            {
                Role = "assistant",
                Content = response
            };
            messages.Add(message);
            if (nameOfCurrentNPC == "Erik")
            {
                erikInteractorScript.ChatLogWithNPC.Add(message);
            }
            if (nameOfCurrentNPC == "Arne")
            {
                arneInteractorScript.ChatLogWithNPC.Add(message);
            }
            if (nameOfCurrentNPC == "Frida")
            {
                fridaInteractorScript.ChatLogWithNPC.Add(message);
            }
            
            //textToSpeech.MakeAudioRequest(response);        //The audio file is created within the MakeAudioRequest method and is stored in the clip variable within this method

            if (response.EndsWith("FINISH_SEN"))
            {
                textToSpeech.MakeAudioRequest(response);        //The audio file is created within the MakeAudioRequest method and is stored in the clip variable within this method
            }
            
            isDone = true;
            
            response = "";
        }

        private void EndConvo()
        {
            //npcDialog.Recover();
            //messages.Clear();
        }
    }
}
