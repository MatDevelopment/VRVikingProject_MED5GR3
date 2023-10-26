using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLookAt : MonoBehaviour
{
    
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private GameObject lookTarget;


    private void OnAnimatorIK()
    {
        if (npcAnimator.enabled)
        {
            npcAnimator.SetLookAtWeight(1, 0, 0.75f, 0.75f, 0.7f);
            npcAnimator.SetLookAtPosition(lookTarget.transform.position);
        }
        else
        {
            npcAnimator.SetLookAtWeight(0);
        }
    }

}
