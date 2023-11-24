using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    //public LevelChanger levelchanger;
    public NPCInteractorScript npcInteractorScript;
        
    public int itemsCollected = 0;

    private string countPrompt_1 = "The Traveller just chose the first personal belonging of Thorsten for Thorsten's funeral. The Traveller needs to choose one more personal belonging. Convey this to the Traveller.";
    private string countPrompt_2 = "The Traveller just chose the last personal belonging of Thorsten for Thorsten's funeral. The Traveller is now ready to go out of the door of the house and go to Thorsten's funeral site. Convey this to the Traveller.";

    private void Start()
    {
        
    }

    void Update()
    {
        /*if (itemsCollected > 0)
        {
            levelchanger.ItemGathered = true;
        }*/
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
