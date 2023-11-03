using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelChanger : MonoBehaviour
{
    // Call for animator object in Unity (insert the animator called 'LevelChanger' into inspector)
    public Animator animator;

    // Variable for loading certain level (based on level index)
    private int levelToLoad;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeToLevel(1);
        }
    }

    // Method for running animation and setting level to load.
    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    // Function used as animation event
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
