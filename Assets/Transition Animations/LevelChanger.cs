using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelChanger : MonoBehaviour
{
    // bool to unlock test conditions
    [SerializeField] bool ReloadSceneTest = true;
    [SerializeField] bool NewSceneTest = false;

    // Call for animator object in Unity (insert the animator called 'LevelChanger' into inspector)
    public Animator animator;

    // Variable for loading certain level (based on level index)
    private int levelToLoad;

    //Determine current scene
        [HideInInspector] public bool Scene1Active = true;
            // Catch bools from other script
                //StonesLoaded;
                //WoodChopped;

        [HideInInspector] public bool Scene2Active = false;
            // Catch bools from other script
                //ItemGathered;

        [HideInInspector] public bool Scene3Active = false;
            // Catch bools from other script
                //StonesPlaced;
                //TorchLit;

    void Awake()
    {
        // This condition only applies when testing.
        if (ReloadSceneTest == true || NewSceneTest == true)
        {
            Scene1Active = false;
            Scene2Active = false;
            Scene3Active = false;
        }

        // Debug.Log("Scene 1 Is Now Active!");
    }

    // Update is called once per frame
    void Update()
    {
        // These conditions only apply when testing. 
        if (Input.GetMouseButtonDown(0) && ReloadSceneTest == true && NewSceneTest == false)
        {
            FadeToLevel(1);
            Debug.Log("Reloading Same Scene!");
        }

        if (Input.GetMouseButtonDown(0) && NewSceneTest == true && ReloadSceneTest == false)
        {
            FadeToNextLevel();
            Debug.Log("Entering New Scene!");
        }

        else if (NewSceneTest == true && ReloadSceneTest == true && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Can't Reload And Start New Scene At The Same Time!");
        }


        // Conditions for interactive experience
        /*
        if (Scene1Active == true)
        {
            if(StonesLoaded == true && WoodChopped == true)
            {
                FadeToNextLevel();
                Scene2Active = true;
                Scene1Active = false;
                Debug.Log("Scene 2 Is Now Active!");
            }
        }

        if (Scene2Active == true)
        {
            if(ItemGathered == true)
            {
                FadeToNextLevel();
                Scene3Active = true;
                Scene2Active = false;
                Debug.Log("Scene 3 Is Now Active!");
            }
        }

        if (Scene3Active == true)
        {
            if(StonesPlaced == true && TorchLit == true)
            {
                FadeToNextLevel();
                Debug.Log("Experience Is Over!");
            }
        }
        
        */
    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method for running animation and setting the level index to load.
    public void FadeToLevel(int levelIndex)
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
