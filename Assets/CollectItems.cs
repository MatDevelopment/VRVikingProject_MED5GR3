using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    public LevelChanger levelChanger;
    public NPCInteractorScript npcInteractorScript;
        
    public int itemsCollected = 0;

    private string countPrompt_1 = "The Traveller just chose the first personal belonging of Thorsten for Thorsten's funeral. The Traveller needs to choose one more personal belonging. Convey this to the Traveller.";
    private string countPrompt_2 = "The Traveller just chose the last personal belonging of Thorsten for Thorsten's funeral. The Traveller is now ready to go out of the door of the house and go to Thorsten's funeral site. Convey this to the Traveller.";

    void Awake()
    {
        //levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();          //Redundant code execution since the level changer script is already being accessed through the inspector and stored in the variable levelChanger
                                                                                                //Generally you do not want to use Find() method since it's expensive and slows down the program.
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PersonalItem"))
        {
            itemsCollected += 1;
            
            switch (itemsCollected)
            {
                case 1:         //If the player has put a single personal belonging in the basket, then a prompt will be sent to the API instructing it to count the amount of picked items.
                    npcInteractorScript.MakeNpcCountRemainingBelongings(countPrompt_1);
                    break;
                case 2:
                    npcInteractorScript.MakeNpcCountRemainingBelongings(countPrompt_2);
                    levelChanger.AllItemGathered = true;
                    break;
            }
        }
        
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PersonalItem"))
        {
            itemsCollected -= 1;
        }
    }
}
