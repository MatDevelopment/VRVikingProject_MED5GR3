using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSpace : MonoBehaviour
{
    public ProximityCounter proximityCounter;
    public LevelChanger levelChanger;

    void Awake()
    {
        proximityCounter = GameObject.Find("Proximity").GetComponent<ProximityCounter>();
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInPersonalZone = true;
        }

        if (levelChanger.Scene3Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Harold"))
            {
                proximityCounter.HaroldInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInPersonalZone = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInPersonalZone = true;
        }

        if (levelChanger.Scene3Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Harold"))
            {
                proximityCounter.HaroldInPersonalZone = true;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInPersonalZone = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInPersonalZone = false;
        }
        
        if (levelChanger.Scene3Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInPersonalZone = false;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInPersonalZone = false;
            }
            if (other.gameObject.CompareTag("Harold"))
            {
                proximityCounter.HaroldInPersonalZone = false;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInPersonalZone = false;
            }
        }  
    }
}
