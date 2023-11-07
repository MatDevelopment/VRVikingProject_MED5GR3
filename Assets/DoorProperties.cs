using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorProperties : MonoBehaviour
{
    public LevelChanger levelChanger;
    // Start is called before the first frame update
    void Awake()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (levelChanger.WoodStacked == true && levelChanger.WoodChopped == true)
        {
            levelChanger.OpeningDoor = true;
        }
    }
}
