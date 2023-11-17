using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{

    public TextMeshProUGUI buttonText;
    
    public Dialogue dialogue;

    public Queue<string> questions;

    // Start is called before the first frame update
    void Start()
    {
        questions = new Queue<string>();

        questions.Clear();

        foreach (string question in dialogue.questions)
        {
            questions.Enqueue(question);
        }

        DisplayNextQuestion();
    }

    // Function called when clicking the button to display the next question in the queue
    public void DisplayNextQuestion()
    {
        if (questions.Count == 0) {
            ResetQueue(dialogue);
            return;
        }

        string question = questions.Dequeue(); // Removes the first question in the queue
        buttonText.text = question; // Sets new question as the visible text
    }

    void ResetQueue(Dialogue dialogue)
    {
        questions.Clear();

        foreach (string question in dialogue.questions)
        {
            questions.Enqueue(question);
        }

        DisplayNextQuestion();
    }
}
