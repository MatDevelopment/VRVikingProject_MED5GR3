using System;
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
        [SerializeField] private NPCInteractorScript ingridInteractorScript;
        
        [SerializeField] private TextToSpeech textToSpeech;
        [SerializeField] private LLMversionPlaying LLMversionPlayingScript;
        
        
        public UnityEvent OnReplyReceived;
        
        //public bool isDone = true;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        public List<ChatMessage> messages = new List<ChatMessage>();

        public AudioClip[] currentNpcThinkingSoundsArray;

        private void Awake()
        {
            LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
        }

        private void Start()
        {
            //nameOfPreviousNPC = nameOfCurrentNPC;
            //button.onClick.AddListener(SendReply);
            if (nameOfCurrentNPC == "Erik")
            {
                currentNpcThinkingSoundsArray = erikInteractorScript.arrayThinkingNPCsounds;
                textToSpeech.audioSource = erikInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = erikInteractorScript.voiceIDNameThisNpc;
                messages = erikInteractorScript.ChatLogWithNPC;
                
                //Debug.Log("erik");
            }
            if (nameOfCurrentNPC == "Arne")
            {
                currentNpcThinkingSoundsArray = arneInteractorScript.arrayThinkingNPCsounds;
                textToSpeech.audioSource = arneInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = arneInteractorScript.voiceIDNameThisNpc;
                messages = arneInteractorScript.ChatLogWithNPC;
            }
            if (nameOfCurrentNPC == "Frida")
            {
                currentNpcThinkingSoundsArray = fridaInteractorScript.arrayThinkingNPCsounds;
                textToSpeech.audioSource = fridaInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = fridaInteractorScript.voiceIDNameThisNpc;
                messages = fridaInteractorScript.ChatLogWithNPC;
            }
            if (nameOfCurrentNPC == "Ingrid")
            {
                currentNpcThinkingSoundsArray = ingridInteractorScript.arrayThinkingNPCsounds;
                textToSpeech.audioSource = ingridInteractorScript.NPCaudioSource;
                textToSpeech.voiceID_name = ingridInteractorScript.voiceIDNameThisNpc;
                messages = ingridInteractorScript.ChatLogWithNPC;
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

        public void AddPlayerInputToChatLog(string playerInput)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = playerInput
            };
            messages.Add(userMessage);
            switch (nameOfCurrentNPC)
            {
                case "Erik":
                    erikInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Arne":
                    arneInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Frida":
                    fridaInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Ingrid":
                    ingridInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                default:
                    erikInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
            }
            
        }

        public void AddNpcResponseToChatLog(string npcResponse)
        {
            var assistantMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = npcResponse
            };
            
            messages.Add(assistantMessage);
            switch (nameOfCurrentNPC)
            {
                case "Erik":
                    erikInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                case "Arne":
                    arneInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                case "Frida":
                    fridaInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                case "Ingrid":
                    ingridInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                default:
                    erikInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
            }

            //messages.Add(assistantMessage);
        }

        public void AddSystemInstructionToChatLog(string instruction)
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                var message = new ChatMessage()
                {
                    Role = "system",
                    Content = instruction
                };
                messages.Add(message);

                switch (nameOfCurrentNPC)
                {
                    case "Erik":
                        erikInteractorScript.ChatLogWithNPC.Add(message);
                        break;
                    case "Arne":
                        arneInteractorScript.ChatLogWithNPC.Add(message);
                        break;
                    case "Frida":
                        fridaInteractorScript.ChatLogWithNPC.Add(message);
                        break;
                    case "Ingrid":
                        ingridInteractorScript.ChatLogWithNPC.Add(message);
                        break;
                    default:
                        erikInteractorScript.ChatLogWithNPC.Add(message);
                        break;
                }
            }
            /*if (nameOfCurrentNPC == "Erik")
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
            if (nameOfCurrentNPC == "Ingrid")
            {
                ingridInteractorScript.ChatLogWithNPC.Add(message);
            }*/

            //return messages;
        }
        
    }
}
