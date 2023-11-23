using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    public LevelChanger levelchanger;
    public int itemsCollected = 0;
    void Awake()
    {
        levelchanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    void Update()
    {
        if (itemsCollected > 0)
        {
            levelchanger.ItemGathered = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PersonalItem"))
        {
            itemsCollected += 1;
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
