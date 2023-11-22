using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackgroundNPC : MonoBehaviour
{
    NavMeshAgent agent;

    public GameObject[] destinations;
    private int destinationID;
    Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        UpdateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target) < 1)
        {
            NextDestination();
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        target = destinations[destinationID].transform.position;
        agent.SetDestination(target);
    }

    void NextDestination()
    {
        destinationID++;

        if (destinationID >= destinations.Length)
        {
            destinationID = 0;
        }
    }
}
