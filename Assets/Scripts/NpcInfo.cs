using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Occupation
{
    Fisherman,
    Farmer,
    Mom,
    Ninth_Century_Wares_Trader,
    Livestock_Trader,
    Slave,
    Norse_Mythology_Preacher
    
}

public enum Talent
{
    Painting,
    Dancing,
    Fishing,
    Hunting,
    Fighting,
    Cooking
}

public enum Personality
{
    Cynical,
    Social,
    Political,
    Opportunist,
    Artistic,
    Kind,
    Altruistic,
    Naive,
    Smart,
    Funny,
    Confident
}

public class NpcInfo : MonoBehaviour
{
    [SerializeField] private string npcName = "";
    [SerializeField] private Occupation npcOccupation;
    [SerializeField] private Talent npcTalents;
    [SerializeField] private Personality npcPersonality;

    public string GetPrompt()
    {
        return $"NPC Name: {npcName}\n" +
               $"NPC Occupation: {npcOccupation.ToString()}\n" +
               $"NPC Talent: {npcTalents.ToString()}\n" +
               $"NPC Personality: {npcPersonality.ToString()}\n";
    }
}
