using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWood : MonoBehaviour
{
    public LevelChanger levelChanger;
    public int GatheredWood = 0; 
    // Start is called before the first frame update
    void Start()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelChanger.countStackedWood >= 2 && levelChanger.WoodStacked == false)
        {
            //levelChanger.CountWoodStacked();
            levelChanger.WoodStacked = true;
        }
        else
        {
            levelChanger.WoodStacked = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChoppedLog"))
        {
            levelChanger.countStackedWood += 1;
        }

        else if (other.CompareTag("WoodLog"))
        {
            Debug.Log("Wood Has Not Been Chopped!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ChoppedLog"))
        {
            levelChanger.countStackedWood -= 1;
        }
    }
}
