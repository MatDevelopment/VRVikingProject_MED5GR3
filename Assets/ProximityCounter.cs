using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCounter : MonoBehaviour
{
    public GameObject ProximitySpaces;
    public LevelChanger levelChanger;

    [Header ("Zone Management:")]
        //Erik
        [Header ("Erik Zones")]
        // Spaces
        public bool ErikInIntimateZone = false;
        public bool ErikInPersonalZone = false;
        public bool ErikInSocialZone = false;

        // Timers
        private float ErikTimerIntimate = 0.0f;
        private float ErikTimerPersonal = 0.0f;
        private float ErikTimerSocial = 0.0f;
        public int ErikTimeInIntimate = 0;
        public int ErikTimeInPersonal = 0;
        public int ErikTimeInSocial = 0;

        //Frida
        [Header ("Frida Zones")]
        // Spaces
        public bool FridaInIntimateZone = false;
        public bool FridaInPersonalZone = false;
        public bool FridaInSocialZone = false;
        
        // Timers
        private float FridaTimerIntimate = 0.0f;
        private float FridaTimerPersonal = 0.0f;
        private float FridaTimerSocial = 0.0f;
        public int FridaTimeInIntimate = 0;
        public int FridaTimeInPersonal = 0;
        public int FridaTimeInSocial = 0;

        //Harold
        [Header ("Harold Zones")]
        // Spaces
        public bool HaroldInIntimateZone = false;
        public bool HaroldInPersonalZone = false;
        public bool HaroldInSocialZone = false;
        
        // Timers
        private float HaroldTimerIntimate = 0.0f;
        private float HaroldTimerPersonal = 0.0f;
        private float HaroldTimerSocial = 0.0f;
        public int HaroldTimeInIntimate = 0;
        public int HaroldTimeInPersonal = 0;
        public int HaroldTimeInSocial = 0;

        //Ingrid
        [Header ("Ingrid Zones")]
        // Spaces
        public bool IngridInIntimateZone = false;
        public bool IngridInPersonalZone = false;
        public bool IngridInSocialZone = false;

        // Timers
        private float IngridTimerIntimate = 0.0f;
        private float IngridTimerPersonal = 0.0f;
        private float IngridTimerSocial = 0.0f;
        public int IngridTimeInIntimate = 0;
        public int IngridTimeInPersonal = 0;
        public int IngridTimeInSocial = 0;

        //Arne
        [Header ("Arne Zones")]
        // Spaces
        public bool ArneInIntimateZone = false;
        public bool ArneInPersonalZone = false;
        public bool ArneInSocialZone = false;

        // Timers
        private float ArneTimerIntimate = 0.0f;
        private float ArneTimerPersonal = 0.0f;
        private float ArneTimerSocial = 0.0f;
        public int ArneTimeInIntimate = 0;
        public int ArneTimeInPersonal = 0;
        public int ArneTimeInSocial = 0;

    void Awake()
    {
        // Find Proximity Object
        ProximitySpaces = GameObject.Find("Proximity");

        // Find levelChanger
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();

        // Saving Values
        //Erik
            // Timers
            ErikTimerIntimate = ErikTimerIntimate;
            ErikTimerPersonal = ErikTimerPersonal;
            ErikTimerSocial = ErikTimerSocial;
            ErikTimeInIntimate = ErikTimeInIntimate;
            ErikTimeInPersonal = ErikTimeInPersonal;
            ErikTimeInSocial = ErikTimeInSocial;

        if(levelChanger.Scene3Active == true)
        {
            //Frida
            // Timers
            FridaTimerIntimate = FridaTimerIntimate;
            FridaTimerPersonal = FridaTimerPersonal;
            FridaTimerSocial = FridaTimerSocial;
            FridaTimeInIntimate = FridaTimeInIntimate;
            FridaTimeInPersonal = FridaTimeInPersonal;
            FridaTimeInSocial = FridaTimeInSocial;

        //Harold    
            // Timers
            HaroldTimerIntimate = HaroldTimerIntimate;
            HaroldTimerPersonal = HaroldTimerPersonal;
            HaroldTimerSocial = HaroldTimerSocial;
            HaroldTimeInIntimate = HaroldTimeInIntimate;
            HaroldTimeInPersonal = HaroldTimeInPersonal;
            HaroldTimeInSocial = HaroldTimeInSocial;

        //Ingrid
            // Timers
            IngridTimerIntimate = IngridTimerIntimate;
            IngridTimerPersonal = IngridTimerPersonal;
            IngridTimerSocial = IngridTimerSocial;
            IngridTimeInIntimate = IngridTimeInIntimate;
            IngridTimeInPersonal = IngridTimeInPersonal;
            IngridTimeInSocial = IngridTimeInSocial;

        //Arne
            // Timers
            ArneTimerIntimate = ArneTimerIntimate;
            ArneTimerPersonal = ArneTimerPersonal;
            ArneTimerSocial = ArneTimerSocial;
            ArneTimeInIntimate = ArneTimeInIntimate;
            ArneTimeInPersonal = ArneTimeInPersonal;
            ArneTimeInSocial = ArneTimeInSocial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ErikCounter();

        if (levelChanger.Scene3Active == true)
        {
            FridaCounter();
            HaroldCounter();
            IngridCounter();
            ArneCounter();
        }
    }

    void ErikCounter()
    {
        //When in social zone, add time to social timer
        if (ErikInSocialZone == true && ErikInPersonalZone == false && ErikInIntimateZone == false)
        {
            ErikTimerSocial += Time.deltaTime; 
            ErikTimeInSocial = (int) (ErikTimerSocial % 60);
        }

        //When in Personal zone, add time to Personal timer
        else if (ErikInSocialZone == true && ErikInPersonalZone == true && ErikInIntimateZone == false)
        {
            ErikTimerPersonal += Time.deltaTime;
            ErikTimeInPersonal = (int) (ErikTimerPersonal % 60);
        }

        //When in intimate zone, add time to intimate timer
        else if (ErikInSocialZone == true && ErikInPersonalZone == true && ErikInIntimateZone == true)
        {
            ErikTimerIntimate += Time.deltaTime;
            ErikTimeInIntimate = (int) (ErikTimerIntimate % 60);
        }
    }
    void FridaCounter()
    {
        //When in social zone, add time to social timer
        if (FridaInSocialZone == true && FridaInPersonalZone == false && FridaInIntimateZone == false)
        {
            FridaTimerSocial += Time.deltaTime; 
            FridaTimeInSocial = (int) (FridaTimerSocial % 60);
        }

        //When in Personal zone, add time to Personal timer
        else if (FridaInSocialZone == true && FridaInPersonalZone == true && FridaInIntimateZone == false)
        {
            FridaTimerPersonal += Time.deltaTime;
            FridaTimeInPersonal = (int) (FridaTimerPersonal % 60);
        }

        //When in intimate zone, add time to intimate timer
        else if (FridaInSocialZone == true && FridaInPersonalZone == true && FridaInIntimateZone == true)
        {
            FridaTimerIntimate += Time.deltaTime;
            FridaTimeInIntimate = (int) (FridaTimerIntimate % 60);
        }
    }
    void HaroldCounter()
    {
        //When in social zone, add time to social timer
        if (HaroldInSocialZone == true && HaroldInPersonalZone == false && HaroldInIntimateZone == false)
        {
            HaroldTimerSocial += Time.deltaTime; 
            HaroldTimeInSocial = (int) (HaroldTimerSocial % 60);
        }

        //When in Personal zone, add time to Personal timer
        else if (HaroldInSocialZone == true && HaroldInPersonalZone == true && HaroldInIntimateZone == false)
        {
            HaroldTimerPersonal += Time.deltaTime;
            HaroldTimeInPersonal = (int) (HaroldTimerPersonal % 60);
        }

        //When in intimate zone, add time to intimate timer
        else if (HaroldInSocialZone == true && HaroldInPersonalZone == true && HaroldInIntimateZone == true)
        {
            HaroldTimerIntimate += Time.deltaTime;
            HaroldTimeInIntimate = (int) (HaroldTimerIntimate % 60);
        }
    }
    void IngridCounter()
    {
        //When in social zone, add time to social timer
        if (IngridInSocialZone == true && IngridInPersonalZone == false && IngridInIntimateZone == false)
        {
            IngridTimerSocial += Time.deltaTime; 
            IngridTimeInSocial = (int) (IngridTimerSocial % 60);
        }

        //When in Personal zone, add time to Personal timer
        else if (IngridInSocialZone == true && IngridInPersonalZone == true && IngridInIntimateZone == false)
        {
            IngridTimerPersonal += Time.deltaTime;
            IngridTimeInPersonal = (int) (IngridTimerPersonal % 60);
        }

        //When in intimate zone, add time to intimate timer
        else if (IngridInSocialZone == true && IngridInPersonalZone == true && IngridInIntimateZone == true)
        {
            IngridTimerIntimate += Time.deltaTime;
            IngridTimeInIntimate = (int) (IngridTimerIntimate % 60);
        }
    }
    void ArneCounter()
    {
        //When in social zone, add time to social timer
        if (ArneInSocialZone == true && ArneInPersonalZone == false && ArneInIntimateZone == false)
        {
            ArneTimerSocial += Time.deltaTime; 
            ArneTimeInSocial = (int) (ArneTimerSocial % 60);
        }

        //When in Personal zone, add time to Personal timer
        else if (ArneInSocialZone == true && ArneInPersonalZone == true && ArneInIntimateZone == false)
        {
            ArneTimerPersonal += Time.deltaTime;
            ArneTimeInPersonal = (int) (ArneTimerPersonal % 60);
        }

        //When in intimate zone, add time to intimate timer
        else if (ArneInSocialZone == true && ArneInPersonalZone == true && ArneInIntimateZone == true)
        {
            ArneTimerIntimate += Time.deltaTime;
            ArneTimeInIntimate = (int) (ArneTimerIntimate % 60);
        }
    }

}
