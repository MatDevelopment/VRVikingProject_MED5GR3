using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class TextToSpeech1 : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;


    // Start is called before the first frame update
    private async void Start()
    {
        var credentials = new BasicAWSCredentials("AKIATSG7CAVML7KMYRMG", "5jDZ4+VXOLZIgkaLEBd/raRgkOo70+pitDyR/0Yg");
        var client = new AmazonPollyClient(credentials, Amazon.RegionEndpoint.EUCentral1);

        var request = new SynthesizeSpeechRequest()
        {
            Text = "Jeg er en fræk lille pige og mit navn er Karoline. Jeg kan rigtig godt lide pik på min rugbgrødsmad",
            Engine = Engine.Neural,
            VoiceId = VoiceId.Sofie,
            OutputFormat = OutputFormat.Mp3
        };


        var response = await client.SynthesizeSpeechAsync(request);

        //WriteIntoFile(response.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip($"{Application.persistentDataPath}/audio.mp3", AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            //audioSource.clip = clip;
            //audioSource.Play();
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[4 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)     //While there are still bytes to read, we will keep writing this into the File Stream
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }

}
