using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{

    public TextMeshProUGUI buttonText;

    [SerializeField] private InputActionReference buttonPressReference = null;
    public bool isHovered = false;

    public Dialogue dialogue;

    private List<string> questions;
    private List<Sound> answers;

    public AudioSource audioSource;
    public Animator animator;
    public CanvasGroup dialogueCanvas;

    private int currentIndex = 0;
    private float fadeDuration = 2;
    private bool faded = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressReference.action.Enable();
        buttonPressReference.action.performed += SelectQuestion;

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
        StartCoroutine(FadeOut(fadeDuration));
        dialogueCanvas.interactable = false;

        audioSource.clip = answers[currentIndex].clip;

        audioSource.Play(); // Plays the corresponding sound
        faded = true;

        currentIndex += 1; // Changes to next question index
        
        if (currentIndex >= questions.Count) {
            currentIndex = 0; // Resets the index if it goes over the total amount of questions
        }

        string question = questions[currentIndex]; // Sets current question to current index
        buttonText.text = question; // Sets new question as the visible text


    }

    public void SelectQuestion(InputAction.CallbackContext context)
    {
        if (isHovered && context.performed)
        {
            DisplayNextQuestion();
        }
    }

    public void hoverEnter()
    {
        isHovered = true;
    }

    public void hoverExit()
    {
        isHovered= false;
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            animator.SetBool("isTalking", true);
        }
        else
        {
            if (faded == true)
            {
                StartCoroutine(FadeIn(fadeDuration));
                faded = false;
                dialogueCanvas.interactable = true;
            }
            animator.SetBool("isTalking", false);
        }
    }

    IEnumerator FadeIn(float duration)
    {
        for (float f = 0; f <= duration; f += Time.deltaTime)
        {
            dialogueCanvas.alpha = Mathf.Lerp(0f, 1f, f / duration);
            yield return null;
        }
        dialogueCanvas.alpha = 1;
    }

    IEnumerator FadeOut(float duration)
    {
        for (float f = 0; f <= duration; f += Time.deltaTime)
        {
            dialogueCanvas.alpha = Mathf.Lerp(1f, 0f, f / duration);
            yield return null;
        }
        dialogueCanvas.alpha = 0;
    }
}
