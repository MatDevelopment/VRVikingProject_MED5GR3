using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace OpenAI
{
    public class ChatTest : MonoBehaviour
    {
        
        public string nameOfCurrentNPC;
        [SerializeField] private NpcInfo npcInfo;
        [SerializeField] private WorldInfo worldInfo;

        [SerializeField] private NPCInteractorScript erikInteractorScript;
        [SerializeField] private NPCInteractorScript arneInteractorScript;
        [SerializeField] private NPCInteractorScript fridaInteractorScript;
        
        [SerializeField] private TextToSpeech textToSpeech;
        
        
        public UnityEvent OnReplyReceived;
        
        //public bool isDone = true;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        public List<ChatMessage> messages = new List<ChatMessage>();

        private void Start()
        {
            //nameOfPreviousNPC = nameOfCurrentNPC;
            //button.onClick.AddListener(SendReply);
            if (nameOfCurrentNPC == "Erik")
            {
                textToSpeech.audioSource = erikInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = erikInteractorScript.voiceIDNameThisNpc;
                messages = erikInteractorScript.ChatLogWithNPC;
                
                //Debug.Log("erik");
            }
            if (nameOfCurrentNPC == "Arne")
            {
                textToSpeech.audioSource = arneInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = arneInteractorScript.voiceIDNameThisNpc;
                messages = arneInteractorScript.ChatLogWithNPC;
            }
            if (nameOfCurrentNPC == "Frida")
            {
                textToSpeech.audioSource = fridaInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = fridaInteractorScript.voiceIDNameThisNpc;
                messages = fridaInteractorScript.ChatLogWithNPC;
            }
            
            
        }
        
        public async Task<string> AskChatGPT(List<ChatMessage> combinedMessages)
        {
            CreateChatCompletionRequest request = new CreateChatCompletionRequest();
            request.Messages = combinedMessages;
            request.Model = "gpt-3.5-turbo-16k-0613";
            request.Temperature = 0.5f;
            request.MaxTokens = 256;

            var response = await openai.CreateChatCompletion(request);

            if(response.Choices != null && response.Choices.Count > 0)
            {
                var chatResponse = response.Choices[0].Message;

                return chatResponse.Content;
                
            }

            return null;
        }

        public List<ChatMessage> AddPlayerInputToChatLog(string playerInput)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = playerInput
            };
            messages.Add(userMessage);
            if (nameOfCurrentNPC == "Erik")
            {
                erikInteractorScript.ChatLogWithNPC.Add(userMessage);
            }
            if (nameOfCurrentNPC == "Arne")
            {
                arneInteractorScript.ChatLogWithNPC.Add(userMessage);
            }
            if (nameOfCurrentNPC == "Frida")
            {
                fridaInteractorScript.ChatLogWithNPC.Add(userMessage);
            }

            return messages;
        }

        public void AddNpcResponseToChatLog(string npcResponse)
        {
            var assistantMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = npcResponse
            };
            
            messages.Add(assistantMessage);
            if (nameOfCurrentNPC == "Erik")
            {
                erikInteractorScript.ChatLogWithNPC.Add(assistantMessage);
            }
            if (nameOfCurrentNPC == "Arne")
            {
                arneInteractorScript.ChatLogWithNPC.Add(assistantMessage);
            }
            if (nameOfCurrentNPC == "Frida")
            {
                fridaInteractorScript.ChatLogWithNPC.Add(assistantMessage);
            }

            messages.Add(assistantMessage);
        }

        public List<ChatMessage> AddSystemInstructionToChatLog(string instruction)
        {
            var message = new ChatMessage()
            {
                Role = "system",
                Content = instruction
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

            return messages;
        }
        
    }
}
