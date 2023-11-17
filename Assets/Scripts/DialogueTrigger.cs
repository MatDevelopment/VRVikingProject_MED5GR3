using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class DialogueTrigger : MonoBehaviour
{

    public TextMeshProUGUI buttonText;
    
    public Dialogue dialogue;

    private List<string> questions;
    private List<Sound> answers;

    public AudioSource audioSource;

    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        questions = new List<string>();
        answers = new List<Sound>();

        foreach (string question in dialogue.questions) // Adds all questions into a list of strings
        {
            questions.Add(question);
        }

        foreach (Sound sound in dialogue.answerAudio) // Adds all soundclips to a list of sounds
        {
            answers.Add(sound);
        }

        string currentQuestion = questions[currentIndex]; // Sets current question to index
        buttonText.text = currentQuestion; // Sets new question as the visible text
    }

    // Function called when clicking the button to display the next question in the queue
    public void DisplayNextQuestion()
    {
        audioSource.clip = answers[currentIndex].clip;
        audioSource.Play(); // Plays the corresponding sound

        currentIndex += 1; // Changes to next question index
        
        if (currentIndex >= questions.Count) {
            currentIndex = 0; // Resets the index if it goes over the total amount of questions
        }

        string question = questions[currentIndex]; // Sets current question to current index
        buttonText.text = question; // Sets new question as the visible text
    }
}
